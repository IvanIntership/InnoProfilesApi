using ProfilesApi.Domain.Common;

namespace ProfilesApi.Domain.Entities;

public class Doctor : SoftDeletableEntity
{
    public Guid? AccountId { get; set; }
    public Guid SpecializationId { get; init; }
    public Guid OfficeId { get; set; }
    private DateTime CareerStartDate { get; init; }
    private int GapInMonths { get; set; }
    public string Degree { get; set; }

    public int TotalExperience
    {
        get
        {
            var totalMonth = ((DateTime.UtcNow.Year - CareerStartDate.Year) * 12) + DateTime.UtcNow.Month - CareerStartDate.Month;
            var result = totalMonth - GapInMonths;
            return result < 0 ? 0 : result;
        }
    }

    public Doctor(Guid accountId, Guid specializationId, Guid officeId, DateTime careerStartDate, int gapInMonths, string degree)
    {
        AccountId = accountId;
        SpecializationId = specializationId;
        OfficeId = officeId;
        CareerStartDate = careerStartDate;
        GapInMonths = gapInMonths;
        Degree = degree;
    }
}