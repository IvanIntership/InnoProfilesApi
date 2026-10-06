using System.Linq.Expressions;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using ProfilesApi.Application.Dto.Administrators;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Application.Services;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Exceptions;
using Xunit;

namespace ProfilesApi.UnitTests.Services;

public class AdministratorServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<ILogger<AdministratorService>> _loggerMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly AdministratorService _adminService;

    public AdministratorServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _loggerMock = new Mock<ILogger<AdministratorService>>();
        _publishEndpointMock = new Mock<IPublishEndpoint>();

        _adminService = new AdministratorService(
            _mapperMock.Object,
            _unitOfWorkMock.Object,
            _passwordHasherMock.Object,
            _loggerMock.Object,
            _publishEndpointMock.Object);
    }

    [Fact]
    public async Task CreateAdministratorAsync_EmailExists_ThrowsConflictException()
    {
        var dto = new CreateAdministratorDto { Email = "test@test.com" };
        _unitOfWorkMock.Setup(u => u.Accounts.ExistsAsync(It.IsAny<Expression<Func<Account, bool>>>(), default))
                       .ReturnsAsync((Expression<Func<Account, bool>> expr, CancellationToken ct) =>
                           expr.Compile().Invoke(new Account { Email = "test@test.com" }));

        await Assert.ThrowsAsync<ConflictException>(() => _adminService.CreateAdministratorAsync(dto, Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateAdministratorAsync_OfficeNotFound_ThrowsNotFoundException()
    {
        var dto = new CreateAdministratorDto { Email = "new@test.com", PhoneNumber = "123", OfficeId = Guid.NewGuid() };
        _unitOfWorkMock.Setup(u => u.Accounts.ExistsAsync(It.IsAny<Expression<Func<Account, bool>>>(), default)).ReturnsAsync(false);
        _unitOfWorkMock.Setup(u => u.Offices.ExistsAsync(It.IsAny<Expression<Func<Office, bool>>>(), default)).ReturnsAsync(false);

        await Assert.ThrowsAsync<NotFoundException>(() => _adminService.CreateAdministratorAsync(dto, Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateAdministratorAsync_Success_ReturnsDto()
    {
        var dto = new CreateAdministratorDto { Email = "new@test.com", PhoneNumber = "123", Password = "Pass" };
        var account = new Account { Id = Guid.NewGuid() };
        var admin = new Administrator { Id = Guid.NewGuid() };

        _unitOfWorkMock.Setup(u => u.Accounts.ExistsAsync(It.IsAny<Expression<Func<Account, bool>>>(), default)).ReturnsAsync(false);
        _unitOfWorkMock.Setup(u => u.Offices.ExistsAsync(It.IsAny<Expression<Func<Office, bool>>>(), default)).ReturnsAsync(true);
        _mapperMock.Setup(m => m.Map<Account>(dto)).Returns(account);
        _mapperMock.Setup(m => m.Map<Administrator>(dto)).Returns(admin);
        _passwordHasherMock.Setup(p => p.HashPassword(It.IsAny<string>())).Returns("HashedPass");
        _mapperMock.Setup(m => m.Map<AdministratorDto>(admin)).Returns(new AdministratorDto { Id = admin.Id });

        var result = await _adminService.CreateAdministratorAsync(dto, Guid.NewGuid());

        Assert.NotNull(result);
        _unitOfWorkMock.Verify(u => u.Accounts.Add(account), Times.Once);
        _unitOfWorkMock.Verify(u => u.Administrators.Add(admin), Times.Once);
        _publishEndpointMock.Verify(p => p.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteAdministratorAsync_NotFound_ThrowsNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.Administrators.GetWithDetailsAsync(It.IsAny<Guid>(), default)).ReturnsAsync((Administrator?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _adminService.DeleteAdministratorAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task DeleteAdministratorAsync_LastAdmin_ThrowsConflictException()
    {
        var id = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Administrators.GetWithDetailsAsync(id, default)).ReturnsAsync(new Administrator { Id = id });
        _unitOfWorkMock.Setup(u => u.Administrators.ExistsAsync(It.IsAny<Expression<Func<Administrator, bool>>>(), default)).ReturnsAsync(false);

        await Assert.ThrowsAsync<ConflictException>(() => _adminService.DeleteAdministratorAsync(id));
    }

    [Fact]
    public async Task DeleteAdministratorAsync_Success_DeletesAdminAndAccount()
    {
        var id = Guid.NewGuid();
        var admin = new Administrator { Id = id, Account = new Account() };
        _unitOfWorkMock.Setup(u => u.Administrators.GetWithDetailsAsync(id, default)).ReturnsAsync(admin);
        _unitOfWorkMock.Setup(u => u.Administrators.ExistsAsync(It.IsAny<Expression<Func<Administrator, bool>>>(), default)).ReturnsAsync(true);

        await _adminService.DeleteAdministratorAsync(id);

        _unitOfWorkMock.Verify(u => u.Administrators.Delete(admin), Times.Once);
        _unitOfWorkMock.Verify(u => u.Accounts.Delete(admin.Account), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(default), Times.Once);
    }

    [Fact]
    public async Task GetAdministratorAsync_NotFound_ThrowsNotFoundException()
    {
        _unitOfWorkMock.Setup(u => u.Administrators.GetWithDetailsAsync(It.IsAny<Guid>(), default)).ReturnsAsync((Administrator?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _adminService.GetAdministratorAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task GetAdministratorAsync_Success_ReturnsDto()
    {
        var id = Guid.NewGuid();
        var admin = new Administrator { Id = id };
        _unitOfWorkMock.Setup(u => u.Administrators.GetWithDetailsAsync(id, default)).ReturnsAsync(admin);
        _mapperMock.Setup(m => m.Map<AdministratorDto>(admin)).Returns(new AdministratorDto { Id = id });

        var result = await _adminService.GetAdministratorAsync(id);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }
}