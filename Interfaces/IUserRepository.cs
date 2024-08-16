using e_commerce_course_api.DTOs;
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
        /// True if the login is successful; otherwise, false.
        /// </returns>
        Task<bool> CheckLogin(LoginDto loginDto);

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
        Task<IdentityResult> CreateUserAsync(string name, string email, string password);

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
        Task<AddressDto?> GetAddressByIdAsync(int id);

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
    }
}
