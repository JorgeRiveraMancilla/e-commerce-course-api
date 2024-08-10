namespace e_commerce_course_api.DTOs
{
    /// <summary>
    /// Data Transfer Object for BasketItem.
    /// </summary>
    public class BasketItemDto
    {
        /// <summary>
        /// ProductId.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Price.
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// URL of the image.
        /// </summary>
        public required string ImageUrl { get; set; }

        /// <summary>
        /// Brand.
        /// </summary>
        public required string Brand { get; set; }

        /// <summary>
        /// Type.
        /// </summary>
        public required string Type { get; set; }

        /// <summary>
        /// Quantity.
        /// </summary>
        public int Quantity { get; set; }
    }
}
