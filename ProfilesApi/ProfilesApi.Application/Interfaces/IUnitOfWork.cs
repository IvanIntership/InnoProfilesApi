using System.Data;
using ProfilesApi.Domain.Interfaces;

namespace ProfilesApi.Application.Interfaces;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IAccountRepository Accounts { get; }
    IAdministratorRepository Administrators { get; }
    IDoctorRepository Doctors { get; }
    IOfficeRepository Offices { get; }
    IPatientRepository Patients { get; }
    IPhotoRepository Photos { get; }
    ISpecializationRepository Specializations { get; }
    int Complete();
    Task<int> CompleteAsync(CancellationToken ct = default);
    Task BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken ct = default);
    Task CommitTransactionAsync(CancellationToken ct = default);
    Task RollbackTransactionAsync(CancellationToken ct = default);
}