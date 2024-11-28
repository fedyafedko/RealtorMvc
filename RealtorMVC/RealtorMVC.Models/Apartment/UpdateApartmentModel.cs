namespace RealtorMVC.Models.Apartment;

public class UpdateApartmentModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int NumberRoom { get; set; }
    public double Square { get; set; }
    public int Floor { get; set; }
    public string Price { get; set; } = string.Empty;
}