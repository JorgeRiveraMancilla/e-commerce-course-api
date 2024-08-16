namespace e_commerce_course_api.Entities
{
    /// <summary>
    /// Represents a product item ordered.
    /// </summary>
    public class ProductItemOrdered
    {
        /// <summary>
        /// The unique identifier of the product item ordered.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The unique identifier of the product.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// The name of the product.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The image URL of the product.
        /// </summary>
        public required string ImageUrl { get; set; }
    }
}
