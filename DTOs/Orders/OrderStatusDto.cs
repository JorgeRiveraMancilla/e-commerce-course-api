namespace e_commerce_course_api.DTOs.Orders
{
    /// <summary>
    /// Data Transfer Object for OrderStatus.
    /// </summary>
    public class OrderStatusDto
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
