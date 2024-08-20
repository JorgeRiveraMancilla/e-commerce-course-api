namespace e_commerce_course_api.DTOs.Baskets
{
    /// <summary>
    /// Data Transfer Object for Basket.
    /// </summary>
    public class BasketDto
    {
        /// <summary>
        /// The basket identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The buyer identifier.
        /// </summary>
        public required string BuyerId { get; set; }

        /// <summary>
        /// The items in the basket.
        /// </summary>
        public required List<BasketItemDto> Items { get; set; }
    }
}