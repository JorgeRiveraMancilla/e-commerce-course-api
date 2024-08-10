namespace e_commerce_course_api.DTOs
{
    public class BasketDto
    {
        public int Id { get; set; }
        public required string BuyerId { get; set; }
        public required List<BasketItemDto> Items { get; set; }
    }
}