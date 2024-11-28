using System.ComponentModel.DataAnnotations.Schema;

namespace RealtorMVC.DAL.Entities;

public class Apartment : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int NumberRoom { get; set; }
    public double Square { get; set; } 
    public int Floor {get; set; }
    public string Price { get; set; } = string.Empty;
    
    [ForeignKey(nameof(User))] 
    public Guid UserId { get; set; }
    [ForeignKey(nameof(Address))]
    public Guid AddressId { get; set; }

    public User User { get; set; } = null!;
    public Address Address { get; set; } = null!;
}