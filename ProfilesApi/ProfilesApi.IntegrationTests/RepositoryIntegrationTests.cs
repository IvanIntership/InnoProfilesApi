using ProfilesApi.Domain.Entities;
using ProfilesApi.Infrastructure.Repositories;
using Xunit;

namespace ProfilesApi.IntegrationTests;

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }

[Collection("Database collection")]
public class RepositoryIntegrationTests
{
    private readonly DatabaseFixture _fixture;

    public RepositoryIntegrationTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _fixture.DbContext.ChangeTracker.Clear();
    }

    [Fact]
    public async Task Add_And_GetByIdAsync_ShouldWriteAndReadFromDatabase()
    {
        var repo = new OfficeRepository(_fixture.DbContext);
        var office = DatabaseFixture.CreateEntity<Office>();
        office.Id = Guid.NewGuid();
        office.Address = "Wall Street 1";
        office.PhoneNumber = "123456789";

        repo.Add(office);
        await _fixture.DbContext.SaveChangesAsync();

        var result = await repo.GetByIdAsync(office.Id);

        Assert.NotNull(result);
        Assert.Equal("Wall Street 1", result.Address);
    }

    [Fact]
    public async Task Delete_ShouldRemoveEntityFromDatabase()
    {
        var repo = new OfficeRepository(_fixture.DbContext);
        var office = DatabaseFixture.CreateEntity<Office>();
        office.Id = Guid.NewGuid();

        repo.Add(office);
        await _fixture.DbContext.SaveChangesAsync();

        repo.Delete(office);
        await _fixture.DbContext.SaveChangesAsync();

        var result = await repo.GetByIdAsync(office.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrueWhenEntityExists()
    {
        var repo = new OfficeRepository(_fixture.DbContext);
        var office = DatabaseFixture.CreateEntity<Office>();
        office.Id = Guid.NewGuid();
        office.Address = "Unique Address";

        repo.Add(office);
        await _fixture.DbContext.SaveChangesAsync();

        var exists = await repo.ExistsAsync(o => o.Address == "Unique Address");

        Assert.True(exists);
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnCorrectPaginationAndTotalCount()
    {
        var repo = new OfficeRepository(_fixture.DbContext);

        for (int i = 0; i < 5; i++)
        {
            var office = DatabaseFixture.CreateEntity<Office>();
            office.Id = Guid.NewGuid();
            repo.Add(office);
        }
        await _fixture.DbContext.SaveChangesAsync();

        var (items, totalCount) = await repo.GetPagedAsync(pageNumber: 1, pageSize: 2);

        Assert.Equal(2, items.Count());
        Assert.True(totalCount >= 5);
    }

    [Fact]
    public async Task GetByEmail_ShouldReturnCorrectAccount()
    {
        var repo = new AccountRepository(_fixture.DbContext);
        var account = DatabaseFixture.CreateEntity<Account>();
        account.Id = Guid.NewGuid();
        account.Email = "test@domain.com";

        repo.Add(account);
        await _fixture.DbContext.SaveChangesAsync();

        var result = await repo.GetByEmail("test@domain.com");

        Assert.NotNull(result);
        Assert.Equal(account.Id, result.Id);
    }

    [Fact]
    public async Task GetByPhoneNumber_ShouldReturnCorrectAccount()
    {
        var repo = new AccountRepository(_fixture.DbContext);
        var account = DatabaseFixture.CreateEntity<Account>();
        account.Id = Guid.NewGuid();
        account.PhoneNumber = "+1234567890";

        repo.Add(account);
        await _fixture.DbContext.SaveChangesAsync();

        var result = await repo.GetByPhoneNumber("+1234567890");

        Assert.NotNull(result);
        Assert.Equal(account.Id, result.Id);
    }

    [Fact]
    public async Task SearchByTerm_ShouldMatchCombinedFirstnameAndLastname()
    {
        var repo = new AccountRepository(_fixture.DbContext);
        var account = DatabaseFixture.CreateEntity<Account>();
        account.Id = Guid.NewGuid();
        account.Firstname = "John";
        account.Lastname = "Doe";

        repo.Add(account);
        await _fixture.DbContext.SaveChangesAsync();

        var results = await repo.SearchByTerm("John Doe");

        Assert.Contains(results, a => a.Id == account.Id);
    }

    [Fact]
    public async Task GetByName_ShouldMatchExactFirstOrLastName()
    {
        var repo = new AccountRepository(_fixture.DbContext);
        var account = DatabaseFixture.CreateEntity<Account>();
        account.Id = Guid.NewGuid();
        account.Firstname = "Alice";
        account.Lastname = "Smith";

        repo.Add(account);
        await _fixture.DbContext.SaveChangesAsync();

        var results = await repo.GetByName("Smith");

        Assert.Contains(results, a => a.Id == account.Id);
    }

    [Fact]
    public async Task SearchByTerm_Specialization_ShouldReturnMatchesCaseInsensitive()
    {
        var repo = new SpecializationRepository(_fixture.DbContext);
        var spec = DatabaseFixture.CreateEntity<Specialization>();
        spec.Id = Guid.NewGuid();
        spec.Name = "Neurology";

        repo.Add(spec);
        await _fixture.DbContext.SaveChangesAsync();

        var results = await repo.SearchByTerm("neuro");

        Assert.Contains(results, s => s.Id == spec.Id);
    }

    [Fact]
    public async Task GetAllAsync_WithIncludes_ShouldLoadRelatedData()
    {
        var repo = new AccountRepository(_fixture.DbContext);
        var account = DatabaseFixture.CreateEntity<Account>();
        account.Id = Guid.NewGuid();

        var photo = DatabaseFixture.CreateEntity<Photo>();
        photo.Id = Guid.NewGuid();
        photo.Url = "http://test.com/photo.png";
        account.Photo = photo;

        repo.Add(account);
        await _fixture.DbContext.SaveChangesAsync();

        var results = await repo.GetAllAsync(
            filter: a => a.Id == account.Id,
            cancellationToken: default,
            includesProperties: a => a.Photo!);

        var loadedAccount = results.First();
        Assert.NotNull(loadedAccount.Photo);
        Assert.Equal("http://test.com/photo.png", loadedAccount.Photo.Url);
    }
}