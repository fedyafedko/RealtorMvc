using AutoMapper;
using RealtorMVC.DAL.Entities;
using RealtorMVC.Models.Address;

namespace RealtorMVC.BLL.Profiles;

public class AddressProfile: Profile
{
    public AddressProfile()
    {
        CreateMap<CreateAddressModel, Address>();
        CreateMap<Address, AddressModel>();
    }
}