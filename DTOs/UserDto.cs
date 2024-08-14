namespace e_commerce_course_api.DTOs
{
    /// <summary>
    /// Data Transfer Object for User.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Username.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Basket.
        /// </summary>
        public required BasketDto Basket { get; set; }
    }
}
