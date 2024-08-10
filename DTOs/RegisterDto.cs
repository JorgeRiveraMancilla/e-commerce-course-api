using System.ComponentModel.DataAnnotations;

namespace e_commerce_course_api.DTOs
{
    /// <summary>
    /// Data Transfer Object for Register.
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// Username.
        /// </summary>
        [Length(3, 20)]
        public required string Username { get; set; }

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
