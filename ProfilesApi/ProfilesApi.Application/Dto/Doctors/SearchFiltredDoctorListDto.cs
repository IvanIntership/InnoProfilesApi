namespace ProfilesApi.Application.Dto.Doctors;

public record SearchFilteredDoctorListDto
{
    public string? SearchTerm { get; init; } = null;
    public Guid? SpecializationId { get; init; } = null;
    public Guid? OfficeId { get; init; } = null;
    public int? MinExperienceYears { get; init; } = null;
}