using AutoMapper;
using RealtorMVC.BLL.Interfaces;
using RealtorMVC.DAL.Entities;
using RealtorMVC.DAL.Repositories.Interfaces;
using RealtorMVC.Models.Apartment;
using Microsoft.EntityFrameworkCore;
using RealtorMVC.Common.Exceptions;
using Microsoft.AspNetCore.Hosting;
using RealtorMVC.Common.Configs;

namespace RealtorMVC.BLL.Service;

public class ApartmentService : IApartmentService
{
    private readonly IRepository<Apartment> _apartmentRepository;
    private readonly IRepository<Address> _addressRepository;
    private readonly ApartmentImagesConfig _apartmentImagesConfig;
    private readonly IWebHostEnvironment _env;
    private readonly IMapper _mapper;

    public ApartmentService(IRepository<Apartment> apartmentRepository, IMapper mapper, IRepository<Address> addressRepository, IWebHostEnvironment env, ApartmentImagesConfig apartmentImagesConfig)
    {
        _apartmentRepository = apartmentRepository;
        _mapper = mapper;
        _addressRepository = addressRepository;
        _env = env;
        _apartmentImagesConfig = apartmentImagesConfig;
    }

    public async Task<ApartmentModel> AddApartmentAsync(Guid userId, CreateApartmentModel apartment)
    {
        var entity = _mapper.Map<Apartment>(apartment);
        entity.UserId = userId;

        await _apartmentRepository.InsertAsync(entity);

        return _mapper.Map<ApartmentModel>(entity);
    }

    public async Task<List<ApartmentModel>> GetAllForRealtor(Guid userId)
    {
        var query = _apartmentRepository
            .Include(a => a.Address)
            .Include(a => a.User)
            .Where(apartment => apartment.UserId == userId);

        var result = _mapper.Map<List<ApartmentModel>>(await query.ToListAsync());

        return result;
    }

    public async Task<bool> DeleteApartmentAsync(Guid id)
    {
        var apartment = await _apartmentRepository.FirstOrDefaultAsync(a => a.Id == id)
            ?? throw new NotFoundException("Apartment with this id does not exist");

        var result = await _apartmentRepository.DeleteAsync(apartment);
        return result;
    }

    public async Task<List<ApartmentModel>> GetAll()
    {
        var apartments = await _apartmentRepository
            .Include(a => a.Address)
            .Include(a => a.User)
            .ToListAsync();

        var result = _mapper.Map<List<ApartmentModel>>(apartments);

        foreach (var apartment in result)
        {
            var path = Path.Combine(_env.ContentRootPath, _apartmentImagesConfig.Folder, apartment.Id.ToString());
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                apartment.Images = files.Select(file => string.Format(_apartmentImagesConfig.Path, apartment.Id, Path.GetFileName(file))).ToList();
            }
        }

        return result;
    }

    public async Task<ApartmentModel> GetById(Guid id)
    {
        var apartment = await _apartmentRepository
            .Include(a => a.Address)
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id)
            ?? throw new NotFoundException("Apartment with this id does not exist");

        var result = _mapper.Map<ApartmentModel>(apartment);

        var path = Path.Combine(_env.ContentRootPath, _apartmentImagesConfig.Folder, apartment.Id.ToString());
        if (Directory.Exists(path))
        {
            var files = Directory.GetFiles(path);
            result.Images = files.Select(file => string.Format(_apartmentImagesConfig.Path, apartment.Id, Path.GetFileName(file))).ToList();
        }

        return result;
    }

    public async Task<ApartmentModel> UpdateApartmentAsync(Guid id, UpdateApartmentModel model)
    {
        var apartment = await _apartmentRepository.FirstOrDefaultAsync(a => a.Id == id)
            ?? throw new NotFoundException("Apartment with this id does not exist");

        _mapper.Map(model, apartment);
        await _apartmentRepository.UpdateAsync(apartment);
        return _mapper.Map<ApartmentModel>(apartment);
    }

    public async Task<ApartmentImagesResponse> UploadImagesAsync(Guid userId, UploadApartmentImageRequest request)
    {
        var apartment = await _apartmentRepository
            .FirstOrDefaultAsync(x => x.Id == request.ApartmentId)
            ?? throw new NotFoundException($"Product not found with such id: {request.ApartmentId}");

        if (apartment.UserId != userId)
            throw new RestrictedAccessException("You are not the owner and do not have permission to perform this action.");

        var contetntPath = _env.ContentRootPath;
        var path = Path.Combine(contetntPath, _apartmentImagesConfig.Folder, request.ApartmentId.ToString());

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var result = new ApartmentImagesResponse();
        result.Paths ??= new List<string>();

        foreach (var image in request.Images)
        {
            var fileName = image.FileName;
            var ext = Path.GetExtension(fileName);

            if (!_apartmentImagesConfig.FileExtensions.Contains(ext))
                throw new IncorrectParametersException("Invalid file extension");

            var uniqueSuffix = DateTime.UtcNow.Ticks;
            fileName = $"apartment_{uniqueSuffix}{ext}";
            var filePath = Path.Combine(path, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);

            result.Paths.Add(string.Format(_apartmentImagesConfig.Path, apartment.Id, fileName));
        }

        return result;
    }

    public async Task<bool> DeleteImagesAsync(Guid userId, DeleteApartmentImageRequest request)
    {
        var apartment = await _apartmentRepository
            .FirstOrDefaultAsync(x => x.Id == request.ApartmentId)
            ?? throw new NotFoundException($"Product not found with such id: {request.ApartmentId}");

        if (apartment.UserId != userId)
            throw new RestrictedAccessException("You are not the owner and do not have permission to perform this action.");

        var wwwPath = _env.ContentRootPath;
        var productImegesPath = Path.Combine(wwwPath, _apartmentImagesConfig.Folder);

        var path = Path.Combine(productImegesPath, request.ApartmentId.ToString(), request.Image);

        if (!File.Exists(path))
            throw new NotFoundException("File not found");

        File.Delete(path);

        return true;
    }
}