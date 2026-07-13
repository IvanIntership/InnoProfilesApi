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
            if (DateTime.UtcNow.Day < CareerStartDate.Day)
            {
                --totalMonth; // If current month day is less than day of career start I'll count this month like the previous month 
            }
            var result = totalMonth - GapInMonths;
            return result;
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