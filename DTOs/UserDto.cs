namespace e_commerce_course_api.DTOs
{
    /// <summary>
    /// Data Transfer Object for User.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Username.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Token.
        /// </summary>
        public required string Token { get; set; }

        /// <summary>
        /// Basket.
        /// </summary>
        public required BasketDto Basket { get; set; }
    }
}
