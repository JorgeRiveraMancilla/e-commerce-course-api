namespace e_commerce_course_api.Entities.Baskets
{
    /// <summary>
    /// Represents a basket of products.
    /// </summary>
    public class Basket
    {
        /// <summary>
        /// The unique identifier of the basket.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The unique identifier of the buyer.
        /// </summary>
        public required string BuyerId { get; set; }

        /// <summary>
        /// The items in the basket.
        /// </summary>
        public List<BasketItem> Items { get; set; } = [];
    }
}
