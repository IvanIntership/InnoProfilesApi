namespace ProfilesApi.Domain.Entities;

public class Doctor : SoftDeletableEntity
{
    public Guid? AccountId { get; private set; }
    public Guid SpecializationId { get; init; }
    public Guid OfficeId { get; private set; }
    private DateTime CareerStartDate { get; init; }
    private int GapInMonths { get; set; }
    public string Degree { get; private set; }

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
        
        if(careerStartDate.Year < 1900 || careerStartDate > DateTime.UtcNow) 
            throw new ArgumentOutOfRangeException(nameof(careerStartDate), "Career start date is out of range");
        CareerStartDate = careerStartDate;
        
        if(gapInMonths < 0) 
            throw new ArgumentOutOfRangeException(nameof(gapInMonths), "Gap in months can't be negative");
        GapInMonths = gapInMonths;
        
        if (String.IsNullOrWhiteSpace(degree))
            throw new ArgumentException("Degree name is empty", nameof(degree));
        Degree = degree;
    }

    public void DeactivateAccount()
    {
        AccountId = null;
        Deactivate();
    }

    public void ActivateAccount(Guid accountId)
    {
        AccountId = accountId;
        Activate();
    }

    public void ChangeOffice(Guid officeId)
    {
        OfficeId = officeId;
    }

    public void ChangeGap(int gapInMonths)
    {
        if(gapInMonths < 0) 
            throw new ArgumentOutOfRangeException(nameof(gapInMonths), "Gap in months can't be negative");
        GapInMonths = gapInMonths;
    }

    public void ChangeDegree(string degree)
    {
        if (String.IsNullOrWhiteSpace(degree))
            throw new ArgumentException("Degree name is empty", nameof(degree));
        Degree = degree;
    }
}