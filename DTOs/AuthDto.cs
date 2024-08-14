namespace e_commerce_course_api.DTOs
{
    /// <summary>
    /// Data transfer object for authentication.
    /// </summary>
    public class AuthDto
    {
        /// <summary>
        /// The user's name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The user's email.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// The user's roles.
        /// </summary>
        public required IList<string> Roles { get; set; }

        /// <summary>
        /// The user's token.
        /// </summary>
        public required string Token { get; set; }

        /// <summary>
        /// The user's basket.
        /// </summary>
        public required BasketDto Basket { get; set; }
    }
}
