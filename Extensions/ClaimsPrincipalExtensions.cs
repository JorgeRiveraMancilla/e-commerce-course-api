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
        /// The user identifier if found; otherwise, null.
        /// </returns>
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId is null ? null : int.Parse(userId);
        }

        /// <summary>
        /// Get the user email.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The user email if found; otherwise, null.
        /// </returns>
        public static string? GetEmail(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
