using RealtorMVC.Models.Apartment;

namespace RealtorMVC.BLL.Interfaces;

public interface IApartmentService
{
    Task<ApartmentModel> AddApartmentAsync(Guid userId, CreateApartmentModel apartment);
    Task<ApartmentModel> GetById(Guid id);
    Task<List<ApartmentModel>> GetAll();
    Task<List<ApartmentModel>> GetAllForRealtor(Guid userId);
    Task<bool> DeleteApartmentAsync(Guid id);
    Task<ApartmentModel> UpdateApartmentAsync(Guid id, UpdateApartmentModel apartment);
    Task<ApartmentImagesResponse> UploadImagesAsync(Guid userId, UploadApartmentImageRequest request);
    Task<bool> DeleteImagesAsync(Guid userId, DeleteApartmentImageRequest request);
}
