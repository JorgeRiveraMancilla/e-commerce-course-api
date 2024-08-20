using AutoMapper;
using e_commerce_course_api.DTOs;
using e_commerce_course_api.DTOs.Baskets;
using e_commerce_course_api.Entities;
using e_commerce_course_api.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace e_commerce_course_api.Data.Repositories
{
    /// <summary>
    /// The user repository.
    /// </summary>
    /// <param name="mapper">
    /// The mapper.
    /// </param>
    /// <param name="tokenService">
    /// The token service.
    /// </param>
    /// <param name="userManager">
    /// The user manager.
    /// </param>
    public class UserRepository(
        IMapper mapper,
        ITokenService tokenService,
        UserManager<User> userManager
    ) : IUserRepository
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// The token service.
        /// </summary>
        private readonly ITokenService _tokenService = tokenService;

        /// <summary>
        /// The user manager.
        /// </summary>
        private readonly UserManager<User> _userManager = userManager;

        /// <summary>
        /// Creates a user.
        /// </summary>
        /// <param name="registerDto">
        /// The register data transfer object.
        /// </param>
        /// <returns>
        /// The identity result.
        /// </returns>
        public async Task<IdentityResult> CreateUserAsync(RegisterDto registerDto)
        {
            var user = new User { UserName = registerDto.Name, Email = registerDto.Email };
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return result;

            return await _userManager.AddToRoleAsync(user, "Member");
        }

        /// <summary>
        /// Gets the address by identifier.
        /// </summary>
        /// <param name="id">
        /// The identifier.
        /// </param>
        /// <returns>
        /// The address data transfer object if found; otherwise, null.
        /// </returns>
        /// <exception cref="Exception">
        /// The user is not found.
        /// </exception>
        public async Task<AddressDto?> GetAddressByIdAsync(int id)
        {
            var user =
                await _userManager.FindByIdAsync(id.ToString())
                ?? throw new Exception("Usuario no encontrado.");

            return _mapper.Map<AddressDto>(user.Address);
        }

        /// <summary>
        /// Gets the authentication by identifier.
        /// </summary>
        /// <param name="id">
        /// The identifier.
        /// </param>
        /// <param name="basketDto">
        /// The basket data transfer object.
        /// </param>
        /// <returns>
        /// The authentication data transfer object.
        /// </returns>
        /// <exception cref="Exception">
        /// The user is not found.
        /// </exception>
        public async Task<AuthDto> GetAuthByIdAsync(int id, BasketDto basketDto)
        {
            var user =
                await _userManager.FindByIdAsync(id.ToString())
                ?? throw new Exception("Usuario no encontrado.");

            return new AuthDto
            {
                Name = user.UserName!,
                Email = user.Email!,
                Roles = await _userManager.GetRolesAsync(user),
                Token = await _tokenService.GenerateTokenAsync(user),
                Basket = basketDto,
            };
        }

        /// <summary>
        /// Gets the user by email.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <returns>
        /// The user data transfer object.
        /// </returns>
        /// <exception cref="Exception">
        /// The user is not found.
        /// </exception>
        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user =
                await _userManager.FindByEmailAsync(email)
                ?? throw new Exception("Usuario no encontrado.");

            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="id">
        /// The identifier.
        /// </param>
        /// <returns>
        /// The user data transfer object.
        /// </returns>
        /// <exception cref="Exception">
        /// The user is not found.
        /// </exception>
        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user =
                await _userManager.FindByIdAsync(id.ToString())
                ?? throw new Exception("Usuario no encontrado.");

            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// Checks the login.
        /// </summary>
        /// <param name="loginDto">
        /// The login data transfer object.
        /// </param>
        /// <returns>
        /// The user data transfer object if login is successful; otherwise, null.
        /// </returns>
        public async Task<UserDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
                return null;

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return null;

            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// Updates the address.
        /// </summary>
        /// <param name="addressDto">
        /// The address data transfer object.
        /// </param>
        /// <param name="userId">
        /// The user identifier.
        /// </param>
        /// <returns>
        /// The identity result.
        /// </returns>
        /// <exception cref="Exception">
        /// The user is not found.
        /// </exception>
        public async Task<IdentityResult> UpdateAddressAsync(AddressDto addressDto, int userId)
        {
            var user =
                await _userManager.FindByIdAsync(userId.ToString())
                ?? throw new Exception("Usuario no encontrado.");

            user.Address = _mapper.Map<Address>(addressDto);

            return await _userManager.UpdateAsync(user);
        }
    }
}
