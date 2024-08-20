namespace e_commerce_course_api.DTOs.Orders
{
    /// <summary>
    /// Data Transfer Object for creating an order.
    /// </summary>
    public class CreateOrderDto
    {
        /// <summary>
        /// If the address should be saved.
        /// </summary>
        public bool SaveAddress { get; set; }

        /// <summary>
        /// The address.
        /// </summary>
        public required AddressDto Address { get; set; }
    }
}
