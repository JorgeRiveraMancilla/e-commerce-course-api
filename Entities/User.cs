using Microsoft.AspNetCore.Identity;

namespace e_commerce_course_api.Entities
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public class User : IdentityUser<int>
    {
        /// <summary>
        /// The basket.
        /// </summary>
        public Basket? Basket { get; set; }

        /// <summary>
        /// The address.
        /// </summary>
        public Address? Address { get; set; }
    }
}
