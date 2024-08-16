using AutoMapper;
using e_commerce_course_api.DTOs;
using e_commerce_course_api.Entities;
using e_commerce_course_api.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace e_commerce_course_api.Data.Repositories
{
    /// <summary>
    /// The user repository.
    /// </summary>
    /// <param name="userManager">
    /// The user manager.
    /// </param>
    /// <param name="tokenService">
    /// The token service.
    /// </param>
    /// <param name="mapper">
    /// The mapper.
    /// </param>
    public class UserRepository(
        UserManager<User> userManager,
        ITokenService tokenService,
        IMapper mapper
    ) : IUserRepository
    {
        /// <summary>
        /// The user manager.
        /// </summary>
        private readonly UserManager<User> _userManager = userManager;

        /// <summary>
        /// The token service.
        /// </summary>
        private readonly ITokenService _tokenService = tokenService;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Checks the login.
        /// </summary>
        /// <param name="loginDto">
        /// The login data transfer object.
        /// </param>
        /// <returns>
        /// True if the login is successful; otherwise, false.
        /// </returns>
        public async Task<bool> CheckLogin(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
                return false;

            return await _userManager.CheckPasswordAsync(user, loginDto.Password);
        }

        /// <summary>
        /// Creates a user.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The identity result.
        /// </returns>
        public async Task<IdentityResult> CreateUserAsync(
            string name,
            string email,
            string password
        )
        {
            var user = new User { UserName = name, Email = email };
            var result = await _userManager.CreateAsync(user, password);

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
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null || user.UserName is null || user.Email is null)
                throw new Exception("Usuario no encontrado.");

            return new AuthDto
            {
                Name = user.UserName,
                Email = user.Email,
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
    }
}
