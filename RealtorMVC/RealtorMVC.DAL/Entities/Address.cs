namespace RealtorMVC.DAL.Entities;

public class Address : BaseEntity
{
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string House { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;

    public List<Apartment> Apartments { get; set; } = new();
}
