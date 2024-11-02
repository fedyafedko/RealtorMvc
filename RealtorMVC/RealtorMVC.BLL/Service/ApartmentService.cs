//using AutoMapper;
//using RealtorMVC.BLL.Interfaces;
//using RealtorMVC.DAL.Entities;
//using RealtorMVC.DAL.Repositories.Interfaces;
//using RealtorAPI.Common.DTO.Apartment;

//namespace RealtorMVC.BLL.Service;

//public class ApartmentService : IApartmentService
//{
//    private readonly IRepository<Apartment> _apartmentRepository;
//    private readonly IMapper _mapper;

//    public ApartmentService(IRepository<Apartment> apartmentRepository, IMapper mapper)
//    {
//        _apartmentRepository = apartmentRepository;
//        _mapper = mapper;
//    }

//    public async Task<CreateApartmentDTO> AddApartmentAsync(int userId, CreateApartmentDTO apartment)
//    {
//        var entity = _mapper.Map<Apartment>(apartment);

//        if (await _apartmentRepository.FindAsync(entity.Id) != null)
//            throw new InvalidOperationException("Entity with such key already exists in the database");

//        entity.UserId = userId;
//        await _apartmentRepository.AddAsync(entity);
//        return _mapper.Map<CreateApartmentDTO>(entity);
//    }

//    public List<ApartmentDTO> GetAllForRealtor(int userId)
//    {
//        var listApart = _mapper
//            .Map<List<ApartmentDTO>>(_apartmentRepository.GetAll().Where(apartment => apartment.UserId == userId ));
//        return listApart.ToList();
//    }

//    public async Task<bool> DeleteApartmentAsync(int id)
//    {
//        var apartment = await _apartmentRepository.FindAsync(id);
//        return apartment != null && await _apartmentRepository.DeleteAsync(apartment) > 0;
//    }

//    public List<ApartmentDTO> GetAll()
//    {
//        return _mapper.Map<IEnumerable<ApartmentDTO>>(_apartmentRepository.GetAll()).ToList();
//    }

//    public async Task<ApartmentDTO?> GetById(int id)
//    {
//        var apartment = await _apartmentRepository.GetByApartmentId(id);
//        return _mapper.Map<ApartmentDTO>(apartment);
//    }

//    public async Task<UpdateApartmentDTO> UpdateApartmentAsync(int id, UpdateApartmentDTO apartment)
//    {
//        var entity = await _apartmentRepository.FindAsync(id);
//        if (entity == null)
//            throw new KeyNotFoundException($"Unable to find entity with such key {id}");

//        _mapper.Map(apartment, entity);
//        await _apartmentRepository.UpdateAsync(entity);
//        return _mapper.Map<UpdateApartmentDTO>(entity);
//    }
//}