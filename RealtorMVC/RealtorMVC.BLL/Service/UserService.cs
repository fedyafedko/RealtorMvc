//using AutoMapper;
//using Microsoft.AspNetCore.Identity;
//using RealtorMVC.BLL.Interfaces;
//using RealtorMVC.DAL.Entities;

//namespace RealtorMVC.BLL.Service;
//public class UserService : IUserService
//{
//    private readonly UserManager<User> _userManager;
//    private readonly IMapper _mapper;

//    public UserService(UserManager<User> userManager, IMapper mapper)
//    {
//        _userManager = userManager;
//        _mapper = mapper;
//    }

//    public async Task<UserDTO> GetByIdAsync(int userId)
//    {
//        var user = await _repository.FindAsync(userId);
//        if (user == null)
//            throw new KeyNotFoundException(nameof(userId));

//        return _mapper.Map<UserDTO>(user);
//    }
//}
