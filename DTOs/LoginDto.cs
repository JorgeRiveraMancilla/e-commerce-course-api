using System.ComponentModel.DataAnnotations;

namespace e_commerce_course_api.DTOs
{
    /// <summary>
    /// Data Transfer Object for Login.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// The user's email.
        /// </summary>
        [EmailAddress]
        public required string Email { get; set; }

        /// <summary>
        /// The user's password.
        /// </summary>
        [Length(6, 20)]
        public required string Password { get; set; }
    }
}
