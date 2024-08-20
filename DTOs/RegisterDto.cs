using System.ComponentModel.DataAnnotations;

namespace e_commerce_course_api.DTOs
{
    /// <summary>
    /// Data Transfer Object for Register.
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// The user's name.
        /// </summary>
        [Length(3, 20)]
        public required string Name { get; set; }

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
