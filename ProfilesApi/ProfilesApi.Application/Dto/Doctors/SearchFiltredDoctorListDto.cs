namespace ProfilesApi.Application.Dto.Doctors;

public record SearchFilteredDoctorListDto(
    string? SearchTerm = null,
    Guid? SpecializationId = null,
    Guid? OfficeId = null,
    int? MinExperienceYears = null
);