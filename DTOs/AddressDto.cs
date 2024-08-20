namespace e_commerce_course_api.DTOs
{
    /// <summary>
    /// Data Transfer Object for Address.
    /// </summary>
    public class AddressDto
    {
        /// <summary>
        /// The unique identifier of the address.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The full name of the address.
        /// </summary>
        public required string FullName { get; set; }

        /// <summary>
        /// The main address line.
        /// </summary>
        public required string Address1 { get; set; }

        /// <summary>
        /// The secondary address line.
        /// </summary>
        public required string Address2 { get; set; }

        /// <summary>
        /// The city.
        /// </summary>
        public required string City { get; set; }

        /// <summary>
        /// The state.
        /// </summary>
        public required string State { get; set; }

        /// <summary>
        /// The postal code.
        /// </summary>
        public required string PostalCode { get; set; }

        /// <summary>
        /// The country.
        /// </summary>
        public required string Country { get; set; }
    }
}
