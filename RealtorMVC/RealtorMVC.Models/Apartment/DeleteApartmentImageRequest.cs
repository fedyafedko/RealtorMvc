namespace RealtorMVC.Models.Apartment;

public class DeleteApartmentImageRequest
{
    public Guid ApartmentId { get; set; }
    public string Image { get; set; } = null!;
}
