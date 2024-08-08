namespace e_commerce_course_api.Entities
{
    /// <summary>
    /// Represents an item in a basket.
    /// </summary>
    public class BasketItem
    {
        /// <summary>
        /// The unique identifier of the item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The quantity of the item.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The unique identifier of the product.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// The product.
        /// </summary>
        public required Product Product { get; set; }
        
        /// <summary>
        /// The unique identifier of the basket.
        /// </summary>
        public int BasketId { get; set; }

        /// <summary>
        /// The basket.
        /// </summary>
        public required Basket Basket { get; set; }
    }
}