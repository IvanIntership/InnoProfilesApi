using System.Text.RegularExpressions;

namespace ProfilesApi.Domain.Entities;

public class Office : SoftDeletableEntity
{
    public string Address { get; private set; }
    public string PhoneNumber { get; private set; }
    public Guid? PhotoId { get; private set; }

    private const string PhoneNumberPattern = @"^\+?(\d{1,3})?[-.\s]?(\(?\d{3}\)?[-.\s]?)?(\d[-.\s]?){6,9}\d$"; // Regular expression for phone number.
    
    public Office(string address, string phoneNumber)
    {
        if (String.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address is empty", nameof(address));
        Address = address;
        
        if (String.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number is empty", nameof(phoneNumber));
        
        if (!Regex.IsMatch(phoneNumber, PhoneNumberPattern))
            throw new ArgumentException("Phone number is invalid", nameof(phoneNumber));
        
        PhoneNumber = phoneNumber;
    }
    
    public void ChangePhoneNumber(string phoneNumber)
    {
        if (String.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number is empty", nameof(phoneNumber));
        
        if (!Regex.IsMatch(phoneNumber, PhoneNumberPattern))
            throw new ArgumentException("Phone number is invalid", nameof(phoneNumber));
        
        PhoneNumber = phoneNumber;
    }
    
    public void SetPhoto(Guid photoId)
    {
        PhotoId = photoId;
    }
}