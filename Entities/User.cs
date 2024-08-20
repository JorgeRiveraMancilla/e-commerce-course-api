using e_commerce_course_api.Entities.Orders;
using Microsoft.AspNetCore.Identity;

namespace e_commerce_course_api.Entities
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public class User : IdentityUser<int>
    {
        public override string? UserName
        {
            get => base.UserName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(UserName), "El nombre es obligatorio.");
                }
                base.UserName = value;
            }
        }
        public override string? Email
        {
            get => base.Email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(
                        nameof(Email),
                        "El correo electrónico es obligatorio."
                    );
                }

                base.Email = value;
            }
        }

        /// <summary>
        /// The unique identifier of the address.
        /// </summary>
        public int? AddressId { get; set; }

        /// <summary>
        /// The address of the user.
        /// </summary>
        public Address? Address { get; set; }

        /// <summary>
        /// The orders of the user.
        /// </summary>
        public List<Order> Orders { get; set; } = [];
    }
}
