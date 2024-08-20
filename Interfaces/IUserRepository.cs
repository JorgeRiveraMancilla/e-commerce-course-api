using e_commerce_course_api.DTOs;
using e_commerce_course_api.DTOs.Baskets;
using Microsoft.AspNetCore.Identity;

namespace e_commerce_course_api.Interfaces
{
    /// <summary>
    /// The user repository.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Checks the login.
        /// </summary>
        /// <param name="loginDto">
        /// The login data transfer object.
        /// </param>
        /// <returns>
        /// The user data transfer object if login is successful; otherwise, null.
        /// </returns>
        Task<UserDto?> LoginAsync(LoginDto loginDto);

        /// <summary>
        /// Creates a user.
        /// </summary>
        /// <param name="registerDto">
        /// The register data transfer object.
        /// </param>
        /// <returns>
        /// The identity result.
        /// </returns>
        Task<IdentityResult> CreateUserAsync(RegisterDto registerDto);

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
        Task<AuthDto> GetAuthByIdAsync(int id, BasketDto basketDto);

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
        Task<UserDto> GetUserByEmailAsync(string email);

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
        Task<UserDto> GetUserByIdAsync(int id);

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
        Task<IdentityResult> UpdateAddressAsync(AddressDto addressDto, int userId);
    }
}
