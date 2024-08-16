namespace e_commerce_course_api.Entities
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
        /// The unique identifier of the buyer.
        /// </summary>
        public required string BuyerId { get; set; }

        /// <summary>
        /// The address of the order.
        /// </summary>
        public required Address Address { get; set; }

        /// <summary>
        /// The items in the order.
        /// </summary>
        public DateTime OrderDate { get; set; } = DateTime.Now;
    }
}
