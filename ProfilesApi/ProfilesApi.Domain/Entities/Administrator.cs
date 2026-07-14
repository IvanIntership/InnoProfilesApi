using ProfilesApi.Domain.Common;

namespace ProfilesApi.Domain.Entities;

public class Administrator : SoftDeletableEntity
{
    public Guid AccountId { get; set; }
    public Guid OfficeId { get; set; }
    public DateTime CareerStartDate { get; init; }
    public int GapInMonths { get; set; }

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
    public Administrator(Guid accountId,  Guid officeId, DateTime careerStartDate, int gapInMonths)
    {
        AccountId = accountId;
        OfficeId = officeId;
        CareerStartDate = careerStartDate;
        GapInMonths = gapInMonths;
    }
}