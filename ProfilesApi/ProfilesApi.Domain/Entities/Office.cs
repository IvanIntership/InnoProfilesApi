using System.Text.RegularExpressions;
using ProfilesApi.Domain.Common;

namespace ProfilesApi.Domain.Entities;

public class Office : BaseEntity
{
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public Guid? PhotoId { get; set; }
    
    public Office(string address, string phoneNumber)
    {
        Address = address;
        PhoneNumber = phoneNumber;
    }
}