using AutoMapper;
using e_commerce_course_api.DTOs;
using e_commerce_course_api.Interfaces;

namespace e_commerce_course_api.Data.Repositories
{
    public class AddressRepository(DataContext dataContext, IMapper mapper) : IAddressRepository
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IMapper _mapper = mapper;

        public async Task<AddressDto?> GetAddressByIdAsync(int id)
        {
            var address = await _dataContext.Addresses.FindAsync(id);

            return _mapper.Map<AddressDto>(address);
        }
    }
}
