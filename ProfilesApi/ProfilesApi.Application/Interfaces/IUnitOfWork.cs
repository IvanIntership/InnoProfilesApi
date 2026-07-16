using ProfilesApi.Domain.Interfaces;

namespace ProfilesApi.Application.Interfaces;

public interface IUnitOfWork
{
    IAccountRepository Accounts { get; }
    IAdministratorRepository Administrators { get; }
    IDoctorRepository Doctors { get; }
    IOfficeRepository Offices { get; }
    IPatientRepository Patient { get; }
    IPhotoRepository Photos { get; }
    ISpecializationRepository Specializations { get; }
    int Complete();
    Task<int> CompleteAsync();
}