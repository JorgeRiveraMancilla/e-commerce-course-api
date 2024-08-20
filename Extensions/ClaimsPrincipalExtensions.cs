using System.Security.Claims;

namespace e_commerce_course_api.Extensions
{
    /// <summary>
    /// The extensions for the claims principal.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Get the user identifier.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The user identifier.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the user is not found.
        /// </exception>
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var userId =
                (user.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                ?? throw new Exception("Usuario no encontrado.");

            return int.Parse(userId);
        }

        /// <summary>
        /// Get the email of the user.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The email of the user.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the user is not found.
        /// </exception>
        public static string GetEmail(this ClaimsPrincipal user)
        {
            var email =
                user.FindFirst(ClaimTypes.Email)?.Value
                ?? throw new Exception("Usuario no encontrado.");

            return email;
        }
    }
}
