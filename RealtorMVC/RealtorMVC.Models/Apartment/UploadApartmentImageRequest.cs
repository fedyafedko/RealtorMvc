using Microsoft.AspNetCore.Http;

namespace RealtorMVC.Models.Apartment;

public class UploadApartmentImageRequest
{
    public Guid ApartmentId { get; set; }
    public List<IFormFile> Images { get; set; } = null!;
}