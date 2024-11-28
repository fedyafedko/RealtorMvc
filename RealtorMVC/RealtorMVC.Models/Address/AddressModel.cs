namespace RealtorMVC.Models.Address;

public class AddressModel
{
    public Guid Id { get; set; }
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string House { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}