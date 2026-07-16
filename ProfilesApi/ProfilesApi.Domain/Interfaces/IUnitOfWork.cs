namespace ProfilesApi.Domain.Interfaces;

public interface IUnitOfWork
{
    IAccountRepository Accounts { get; }
    IAdministratorRepository Administrators { get; }
    IDoctorRepository Doctors { get; }
    IOfficeRepository Offices { get; }
    IPatientRepository Patient { get; }
    IPhotoRepository Photos { get; }
    ISpecializationRepository Specializations { get; }

    Task<int> CompleteAsync();
}