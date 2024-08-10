namespace e_commerce_course_api.DTOs
{
    public class BasketItemDto
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public long Price { get; set; }
        public required string ImageUrl { get; set; }
        public required string Brand { get; set; }
        public required string Type { get; set; }
        public int Quantity { get; set; }
    }
}
