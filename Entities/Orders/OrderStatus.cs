namespace e_commerce_course_api.Entities.Orders
{
    /// <summary>
    /// Represents the status of an order.
    /// </summary>
    public class OrderStatus
    {
        /// <summary>
        /// The unique identifier of the status.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the status.
        /// </summary>
        public required string Name { get; set; }
    }
}
