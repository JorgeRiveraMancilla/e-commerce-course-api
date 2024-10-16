namespace e_commerce_course_api.DTOs
{
    /// <summary>
    /// Data Transfer Object for User.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// The user identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The user name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The user email.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// The unique identifier of the address.
        /// </summary>
        public int? AddressId { get; set; }
    }
}
