namespace e_commerce_course_api.Entities
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

        /// <summary>
        /// Adds a product to the basket.
        /// </summary>
        /// <param name="product">
        /// The product to add.
        /// </param>
        /// <param name="quantity">
        /// The quantity of the product to add.
        /// </param>
        public void AddItem(Product product, int quantity)
        {
            var item = Items.FirstOrDefault(x => x.ProductId == product.Id);

            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                Items.Add(
                    new BasketItem
                    {
                        Basket = this,
                        Product = product,
                        Quantity = quantity
                    }
                );
            }
        }

        /// <summary>
        /// Removes a product from the basket.
        /// </summary>
        /// <param name="productId">
        /// The unique identifier of the product to remove.
        /// </param>
        /// <param name="quantity">
        /// The quantity of the product to remove.
        /// </param>
        public void RemoveItem(int productId, int quantity = 1)
        {
            var item = Items.FirstOrDefault(x => x.ProductId == productId);

            if (item != null)
            {
                item.Quantity -= quantity;

                if (item.Quantity == 0)
                {
                    Items.Remove(item);
                }
            }
        }
    }
}
