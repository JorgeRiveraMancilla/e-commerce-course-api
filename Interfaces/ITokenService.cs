using e_commerce_course_api.Entities;

namespace e_commerce_course_api.Interfaces
{
    /// <summary>
    /// Interface for the token service.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates a JWT token for a user.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The JWT token.
        /// </returns>
        Task<string> GenerateTokenAsync(User user);
    }
}
