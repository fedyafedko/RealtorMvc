namespace RealtorMVC.Models.Apartment;

public class DeleteApartmentImageRequest
{
    public Guid ApartmentId { get; set; }
    public List<string> Images { get; set; } = null!;
}
