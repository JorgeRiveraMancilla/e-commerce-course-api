using AutoMapper;
using AutoMapper.QueryableExtensions;
using e_commerce_course_api.DTOs.Baskets;
using e_commerce_course_api.Entities.Baskets;
using e_commerce_course_api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_course_api.Data.Repositories
{
    /// <summary>
    /// The repository for the basket.
    /// </summary>
    /// <param name="dataContext">
    /// The data context to use.
    /// </param>
    /// <param name="mapper">
    /// The mapper to use.
    /// </param>
    public class BasketRepository(DataContext dataContext, IMapper mapper) : IBasketRepository
    {
        /// <summary>
        /// The data context to use.
        /// </summary>
        private readonly DataContext _dataContext = dataContext;

        /// <summary>
        /// The mapper to use.
        /// </summary>
        private readonly IMapper _mapper = mapper;

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
        public async Task<BasketDto> AddItemToBasketAsync(
            string buyerId,
            int productId,
            int quantity
        )
        {
            var basket =
                await _dataContext
                    .Baskets.Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                    .FirstOrDefaultAsync(x => x.BuyerId == buyerId)
                ?? throw new Exception("Carrito no encontrado.");

            var item = basket.Items.FirstOrDefault(x => x.Product.Id == productId);

            if (item is null)
            {
                var product =
                    await _dataContext.Products.FindAsync(productId)
                    ?? throw new Exception("Producto no encontrado.");

                basket.Items.Add(
                    new BasketItem
                    {
                        Product = product,
                        Quantity = quantity,
                        Basket = basket
                    }
                );
            }
            else
            {
                item.Quantity += quantity;
            }

            return _mapper.Map<BasketDto>(basket);
        }

        /// <summary>
        /// Creates a basket.
        /// </summary>
        /// <param name="buyerId">
        /// The unique identifier of the buyer.
        /// </param>
        /// <returns>
        /// The created basket.
        /// </returns>
        public async Task<BasketDto> CreateBasketAsync(string buyerId)
        {
            var basket = new Basket
            {
                BuyerId = buyerId,
                PaymentIntentId = "",
                ClientSecret = "",
            };

            await _dataContext.Baskets.AddAsync(basket);
            return _mapper.Map<BasketDto>(basket);
        }

        /// <summary>
        /// Gets a basket by the buyer id.
        /// </summary>
        /// <param name="buyerId">
        /// The unique identifier of the buyer.
        /// </param>
        /// <returns>
        /// The basket.
        /// </returns>
        public async Task<BasketDto?> GetBasketByBuyerIdAsync(string buyerId)
        {
            return await _dataContext
                .Baskets.Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .ProjectTo<BasketDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.BuyerId == buyerId);
        }

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
        public async Task<BasketDto> RemoveBasketAsync(int id)
        {
            var basket =
                await _dataContext.Baskets.FindAsync(id)
                ?? throw new Exception("Carrito no encontrado.");

            _dataContext.Baskets.Remove(basket);

            return _mapper.Map<BasketDto>(basket);
        }

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
        public async Task<BasketDto> RemoveItemFromBasketAsync(
            int basketId,
            int productId,
            int quantity
        )
        {
            var basket =
                await _dataContext
                    .Baskets.Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                    .FirstOrDefaultAsync(x => x.Id == basketId)
                ?? throw new Exception("Carrito no encontrado.");

            var item =
                basket.Items.FirstOrDefault(x => x.Product.Id == productId)
                ?? throw new Exception("Producto no encontrado.");

            item.Quantity -= quantity;

            if (item.Quantity == 0)
                _dataContext.BasketItems.Remove(item);

            return _mapper.Map<BasketDto>(basket);
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns>
        /// True if the changes were saved; otherwise, false.
        /// </returns>
        public async Task<bool> SaveChangesAsync()
        {
            return 0 < await _dataContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the basket.
        /// </summary>
        /// <param name="basketDto">
        /// The basket data transfer object.
        /// </param>
        /// <returns>
        /// The task.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the basket is not found.
        /// </exception>
        public async Task UpdateBasketAsync(BasketDto basketDto)
        {
            var basket =
                await _dataContext.Baskets.FirstOrDefaultAsync(x => x.Id == basketDto.Id)
                ?? throw new Exception("Carrito no encontrado.");

            basket.BuyerId = basketDto.BuyerId;
            basket.PaymentIntentId = basketDto.PaymentIntentId;
            basket.ClientSecret = basketDto.ClientSecret;
        }

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
        public async Task<BasketDto> UpdateBuyerIdAsync(string oldBuyerId, string newBuyerId)
        {
            var basket =
                await _dataContext
                    .Baskets.Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                    .FirstOrDefaultAsync(x => x.BuyerId == oldBuyerId)
                ?? throw new Exception("Carrito no encontrado.");

            basket.BuyerId = newBuyerId;
            return _mapper.Map<BasketDto>(basket);
        }
    }
}
