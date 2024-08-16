using e_commerce_course_api.DTOs;

namespace e_commerce_course_api.Interfaces
{
    /// <summary>
    /// The repository for the basket.
    /// </summary>
    public interface IBasketRepository
    {
        /// <summary>
        /// Adds an item to the basket.
        /// </summary>
        /// <param name="buyerId">
        /// The unique identifier of the buyer.
        /// </param>
        /// <param name="productId">
        /// The unique identifier of the product.
        /// </param>
        /// <param name="quantity">
        /// The quantity of the product to add.
        /// </param>
        /// <returns>
        /// The basket with the added item.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the basket is not found or the product is not found.
        /// </exception>
        Task<BasketDto> AddItemToBasketAsync(string buyerId, int productId, int quantity = 1);

        /// <summary>
        /// Creates a basket.
        /// </summary>
        /// <param name="buyerId">
        /// The unique identifier of the buyer.
        /// </param>
        /// <returns>
        /// The created basket.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the buyer id is null or empty.
        /// </exception>
        Task<BasketDto> CreateBasketAsync(string buyerId);

        /// <summary>
        /// Gets a basket by the buyer id.
        /// </summary>
        /// <param name="buyerId">
        /// The unique identifier of the buyer.
        /// </param>
        /// <returns>
        /// The basket.
        /// </returns>
        Task<BasketDto?> GetBasketByBuyerIdAsync(string buyerId);

        /// <summary>
        /// Removes a basket.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the basket.
        /// </param>
        /// <returns>
        /// The task.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the basket is not found.
        /// </exception>
        public Task RemoveBasketAsync(int id);

        /// <summary>
        /// Removes an item from the basket.
        /// </summary>
        /// <param name="basketId">
        /// The unique identifier of the basket.
        /// </param>
        /// <param name="productId">
        /// The unique identifier of the product.
        /// </param>
        /// <param name="quantity">
        /// The quantity of the product to remove.
        /// </param>
        /// <returns>
        /// The task.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the basket is not found, the item is not found, or the quantity to be removed exceeds the quantity of the item.
        /// </exception>
        Task RemoveItemFromBasketAsync(int basketId, int productId, int quantity = 1);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns>
        /// True if the changes were saved; otherwise, false.
        /// </returns>
        Task<bool> SaveChangesAsync();

        /// <summary>
        /// Updates the buyer id.
        /// </summary>
        /// <param name="oldBuyerId">
        /// The old buyer id.
        /// </param>
        /// <param name="newBuyerId">
        /// The new buyer id.
        /// </param>
        /// <returns>
        /// The task.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the basket is not found.
        /// </exception>
        Task<BasketDto> UpdateBuyerIdAsync(string oldBuyerId, string newBuyerId);
    }
}
