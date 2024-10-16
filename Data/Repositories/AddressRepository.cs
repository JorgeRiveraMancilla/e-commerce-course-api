using AutoMapper;
using e_commerce_course_api.DTOs;
using e_commerce_course_api.Interfaces;

namespace e_commerce_course_api.Data.Repositories
{
    public class AddressRepository(
        DataContext dataContext,
        IMapper mapper,
        IUserRepository userRepository
    ) : IAddressRepository
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IMapper _mapper = mapper;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<AddressDto?> GetAddressByUserIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user is null || user.AddressId is null)
                return null;

            var address = await _dataContext.Addresses.FindAsync(user.AddressId);

            return _mapper.Map<AddressDto>(address);
        }
    }
}
