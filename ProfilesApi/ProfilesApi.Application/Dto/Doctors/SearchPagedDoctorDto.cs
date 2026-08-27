namespace ProfilesApi.Application.Dto.Doctors;

public sealed record SearchPagedDoctorDto
{
    public string? SearchTerm { get; init; } = null;
    public Guid? SpecializationId { get; init; } = null;
    public Guid? OfficeId { get; init; } = null;
    public int? MinExperienceYears { get; init; } = null;
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}