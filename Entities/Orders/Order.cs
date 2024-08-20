namespace e_commerce_course_api.Entities.Orders
{
    /// <summary>
    /// Represents an order.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// The unique identifier of the order.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The date of the order.
        /// </summary>
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The subtotal of the order.
        /// </summary>
        public long Subtotal { get; set; }

        /// <summary>
        /// The delivery fee of the order.
        /// </summary>
        public long DeliveryFee { get; set; }

        /// <summary>
        /// The total of the order.
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// The total of the order.
        /// </summary>
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        /// <summary>
        /// The user identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The user.
        /// </summary>
        public required User User { get; set; }

        /// <summary>
        /// The unique identifier of the address.
        /// </summary>
        public int AddressId { get; set; }

        /// <summary>
        /// The address of the order.
        /// </summary>
        public required Address Address { get; set; }

        /// <summary>
        /// The items of the order.
        /// </summary>
        public List<OrderItem> OrderItems { get; set; } = [];
    }
}
