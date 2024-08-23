namespace e_commerce_course_api.Entities.Orders
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
        /// The unique identifier of the product.
        /// </summary>
        public required int ProductId { get; set; }

        /// <summary>
        /// The name of the product.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The description of the product.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// The price of the item.
        /// </summary>
        public required long Price { get; set; }

        /// <summary>
        /// The image URL of the product.
        /// </summary>
        public required string ImageUrl { get; set; }

        /// <summary>
        /// The type of the product.
        /// </summary>
        public required string Type { get; set; }

        /// <summary>
        /// The brand of the product.
        /// </summary>
        public required string Brand { get; set; }

        /// <summary>
        /// The quantity of the item.
        /// </summary>
        public required int Quantity { get; set; }

        /// <summary>
        /// The order identifier.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// The order.
        /// </summary>
        public required Order Order { get; set; }
    }
}
