using Microsoft.AspNetCore.Identity;

namespace RealtorMVC.DAL.Entities;

public class User : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Avatar { get; set; }

    public List<Apartment> Apartments { get; set; } = new();
}