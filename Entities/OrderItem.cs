namespace e_commerce_course_api.Entities
{
    /// <summary>
    /// Represents an item in an order.
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// The unique identifier of the item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The unique identifier of the product item ordered.
        /// </summary>
        public required ProductItemOrdered ProductItemOrdered { get; set; }

        /// <summary>
        /// The price of the item.
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// The quantity of the item.
        /// </summary>
        public int Quantity { get; set; }
    }
}
