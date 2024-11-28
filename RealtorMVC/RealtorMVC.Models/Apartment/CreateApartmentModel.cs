using RealtorMVC.Models.Address;

namespace RealtorMVC.Models.Apartment;

public class CreateApartmentModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int NumberRoom { get; set; }
    public double Square { get; set; }
    public int Floor { get; set; }
    public string Price { get; set; } = string.Empty;
    public CreateAddressModel Address { get; set; } = null!;
}
