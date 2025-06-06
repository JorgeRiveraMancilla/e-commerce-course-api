namespace e_commerce_course_api.DTOs.Products
{
    /// <summary>
    /// Data Transfer Object for Product.
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// The unique identifier of the product.
        /// </summary>
        public int Id { get; set; }

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
        public required long Price { get; set; }

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
        /// The stock of the product.
        /// </summary>
        public required int Stock { get; set; }
    }
}
