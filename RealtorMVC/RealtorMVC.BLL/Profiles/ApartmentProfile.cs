using AutoMapper;
using RealtorMVC.DAL.Entities;
using RealtorMVC.Models.Apartment;

namespace RealtorMVC.BLL.Profiles;

public class ApartmentProfile : Profile
{
    public ApartmentProfile()
    {
        CreateMap<CreateApartmentModel, Apartment>();
        CreateMap<UpdateApartmentModel, Apartment>();
        CreateMap<Apartment, ApartmentModel>();
    }
}