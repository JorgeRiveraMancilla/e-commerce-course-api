namespace e_commerce_course_api.DTOs.Orders
{
    /// <summary>
    /// Data Transfer Object for Order.
    /// </summary>
    public class OrderDto
    {
        /// <summary>
        /// The order identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The date of the order.
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// The subtotal of the order.
        /// </summary>
        public required long Subtotal { get; set; }

        /// <summary>
        /// The delivery fee of the order.
        /// </summary>
        public required long DeliveryFee { get; set; }

        /// <summary>
        /// The total of the order.
        /// </summary>
        public required long Total { get; set; }

        /// <summary>
        /// The status of the order.
        /// </summary>
        public string? OrderStatus { get; set; }

        /// <summary>
        /// The address of the order.
        /// </summary>
        public required AddressDto Address { get; set; }

        /// <summary>
        /// The items in the order.
        /// </summary>
        public required List<OrderItemDto> OrderItems { get; set; }
    }
}
