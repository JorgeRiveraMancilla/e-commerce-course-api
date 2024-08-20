namespace e_commerce_course_api.DTOs.Baskets
{
    /// <summary>
    /// Data Transfer Object for BasketItem.
    /// </summary>
    public class BasketItemDto
    {
        /// <summary>
        /// The unique identifier of the product.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// The name of the product.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The description of the product.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// The price of the product.
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// The URL of the image of the product.
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
        public int Quantity { get; set; }
    }
}
