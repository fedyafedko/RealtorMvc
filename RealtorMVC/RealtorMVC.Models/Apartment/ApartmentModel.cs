using RealtorMVC.Models.Address;
using RealtorMVC.Models.User;

namespace RealtorMVC.Models.Apartment;

public class ApartmentModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int NumberRoom { get; set; }
    public double Square { get; set; }
    public int Floor { get; set; }
    public string Price { get; set; } = string.Empty;
    public List<string> Images { get; set; } = null!;

    public UserModel User { get; set; } = null!;
    public AddressModel Address { get; set; } = null!;
}