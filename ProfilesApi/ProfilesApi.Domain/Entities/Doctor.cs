using ProfilesApi.Domain.Common;

namespace ProfilesApi.Domain.Entities;

public class Doctor : SoftDeletableEntity
{
    public Guid AccountId { get; set; }
    public virtual Account Account { get; set; }
    public Guid SpecializationId { get; init; }
    public virtual Specialization Specialization { get; set; }
    public Guid OfficeId { get; set; }
    public virtual Office Office { get; set; }
    public DateTime CareerStartDate { get; init; }
    public int GapInMonths { get; set; }
    public string Degree { get; set; }

    public int TotalExperience
    {
        get
        {
            var totalMonth = ((DateTime.UtcNow.Year - CareerStartDate.Year) * 12) + DateTime.UtcNow.Month - CareerStartDate.Month;
            if (DateTime.UtcNow.Day < CareerStartDate.Day)
            {
                --totalMonth;
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