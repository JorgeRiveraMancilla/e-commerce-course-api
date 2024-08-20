namespace e_commerce_course_api.DTOs.Orders
{
    /// <summary>
    /// Data Transfer Object for Order Item.
    /// </summary>
    public class OrderItemDto
    {
        /// <summary>
        /// The product identifier.
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
        /// The quantity of the product.
        /// </summary>
        public required int Quantity { get; set; }
    }
}
