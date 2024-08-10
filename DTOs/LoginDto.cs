using System.ComponentModel.DataAnnotations;

namespace e_commerce_course_api.DTOs
{
    /// <summary>
    /// Data Transfer Object for Login.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Email.
        /// </summary>
        [EmailAddress]
        public required string Email { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        [Length(6, 20)]
        public required string Password { get; set; }
    }
}
