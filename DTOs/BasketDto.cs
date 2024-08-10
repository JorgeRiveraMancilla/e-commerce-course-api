namespace e_commerce_course_api.DTOs
{
    /// <summary>
    /// Data Transfer Object for Basket.
    /// </summary>
    public class BasketDto
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// BuyerId.
        /// </summary>
        public required string BuyerId { get; set; }

        /// <summary>
        /// Items.
        /// </summary>
        public required List<BasketItemDto> Items { get; set; }
    }
}
