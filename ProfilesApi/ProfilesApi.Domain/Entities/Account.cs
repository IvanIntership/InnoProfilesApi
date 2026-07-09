using System.Text.RegularExpressions;
using ProfilesApi.Domain.Enums;

namespace ProfilesApi.Domain.Entities;

public class Account : SoftDeletableEntity
{
    public string Firstname { get; private set; }
    public string Lastname { get; private set; }
    public DateTime Birthday { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public Roles Role { get; private set; }
    public Guid? PhotoId { get; private set; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; private set; }
    public string CreatedBy { get; init; }
    public string UpdatedBy { get; private set; }
    
    private const string PhoneNumberPattern = @"^\+?(\d{1,3})?[-.\s]?(\(?\d{3}\)?[-.\s]?)?(\d[-.\s]?){6,9}\d$"; // Regular expression for phone number.
    private const string EmailPattern = @"([a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\.[a-zA-Z0-9_-]+)"; // Regular expression for Email.
    
    public Account(string firstname, string lastname, DateTime birthday, string phoneNumber,  string email, string passwordHash, Roles role, string createdBy, string updatedBy)
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        
        if(String.IsNullOrEmpty(firstname) || String.IsNullOrEmpty(lastname))
            throw new ArgumentException("First name and last name cannot be empty");
        Firstname = firstname;
        Lastname = lastname;
        
        if(birthday.Year < 1900 || birthday > DateTime.UtcNow) 
            throw new ArgumentOutOfRangeException(nameof(birthday), "Birthday is out of range");
        Birthday = birthday;
        
        if (String.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number is empty", nameof(phoneNumber));
        if (!Regex.IsMatch(phoneNumber, PhoneNumberPattern))
            throw new ArgumentException("Phone number is invalid", nameof(phoneNumber));
        PhoneNumber = phoneNumber;
        
        if (String.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is empty", nameof(email));
        if (!Regex.IsMatch(email, EmailPattern))
            throw new ArgumentException("Email is invalid", nameof(email));
        Email = email;
        
        if (passwordHash.Length < 6)
            throw new ArgumentException("Password is too short", nameof(passwordHash));
        PasswordHash = passwordHash;
        
        Role = role;
        CreatedBy = createdBy;
        UpdatedBy = updatedBy;
    }
    
    public void ChangeFirstname(string firstname, string updatedBy)
    {
        if(String.IsNullOrEmpty(firstname))
            throw new ArgumentException("First name cannot be empty");
        Firstname = firstname;
        Update(updatedBy);
    }

    public void ChangeLastname(string lastname, string updatedBy)
    {
        if(String.IsNullOrEmpty(lastname))
            throw new ArgumentException("Last name cannot be empty");
        Lastname = lastname;
        Update(updatedBy);
    }

    public void ChangePhoneNumber(string phoneNumber, string updatedBy)
    {
        if (String.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number is empty", nameof(phoneNumber));
        if (!Regex.IsMatch(phoneNumber, PhoneNumberPattern))
            throw new ArgumentException("Phone number is invalid", nameof(phoneNumber));
        PhoneNumber = phoneNumber;
        Update(updatedBy);
    }

    public void ChangeBirthday(DateTime birthday, string updatedBy)
    {
        if(birthday.Year < 1900 || birthday > DateTime.UtcNow) 
            throw new ArgumentOutOfRangeException(nameof(birthday), "Birthday is out of range");
        Birthday = birthday;
        Update(updatedBy);
    }

    public void ChangeEmail(string email, string updatedBy)
    {
        if (String.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is empty", nameof(email));
        if (!Regex.IsMatch(email, EmailPattern))
            throw new ArgumentException("Email is invalid", nameof(email));
        Email = email;
        Update(updatedBy);
    }

    public void ChangePhoto(Guid photoId, string updatedBy)
    {
        PhotoId = photoId;
        Update(updatedBy);
    }

    public void ChangePassword(string passwordHash, string updatedBy)
    {
        PasswordHash = passwordHash;
        Update(updatedBy);
    }

    private void Update(string updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedAt = DateTime.UtcNow;
    }
}