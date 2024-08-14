using Microsoft.AspNetCore.Identity;

namespace e_commerce_course_api.Entities
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public class User : IdentityUser<int>
    {
        public Basket? Basket { get; set; }
    }
}
