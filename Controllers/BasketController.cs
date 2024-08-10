using e_commerce_course_api.Data;
using e_commerce_course_api.DTOs;
using e_commerce_course_api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_course_api.Controllers
{
    /// <summary>
    /// The controller for the basket.
    /// </summary>
    /// <param name="dataContext">
    /// The data context to use.
    /// </param>
    public class BasketController(DataContext dataContext) : BaseApiController
    {
        /// <summary>
        /// The data context to use.
        /// </summary>
        private readonly DataContext _dataContext = dataContext;

        /// <summary>
        /// Get the basket.
        /// </summary>
        /// <returns>
        /// The basket.
        /// </returns>
        [HttpGet(Name = "GetBasket")]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await RetrieveBasketAsync(GetBuyerId());

            if (basket is null)
                return NotFound();

            return Ok(MapBasketToDto(basket));
        }

        /// <summary>
        /// Add an item to the basket.
        /// </summary>
        /// <param name="productId">
        /// The id of the product to add.
        /// </param>
        /// <param name="quantity">
        /// The quantity of the product to add.
        /// </param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> AddItemToBasket(int productId, int quantity = 1)
        {
            var basket = await RetrieveBasketAsync(GetBuyerId()) ?? await CreateBasketAsync();
            var product = await _dataContext.Products.FindAsync(productId);

            if (product is null)
                return BadRequest(new ProblemDetails { Title = "Producto no encontrado." });

            basket.AddItem(product, quantity);

            var result = 0 < await _dataContext.SaveChangesAsync();

            if (!result)
                return BadRequest(
                    new ProblemDetails { Title = "Error al añadir el producto al carrito." }
                );

            return CreatedAtRoute("GetBasket", MapBasketToDto(basket));
        }

        /// <summary>
        /// Remove an item from the basket.
        /// </summary>
        /// <param name="productId">
        /// The id of the product to remove.
        /// </param>
        /// <param name="quantity">
        /// The quantity of the product to remove.
        /// </param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        [HttpDelete]
        public async Task<ActionResult> RemoveBasketItem(int productId, int quantity = 1)
        {
            var basket = await RetrieveBasketAsync(GetBuyerId());

            if (basket is null)
                return NotFound();

            basket.RemoveItem(productId, quantity);

            var result = 0 < await _dataContext.SaveChangesAsync();

            if (!result)
                return BadRequest(
                    new ProblemDetails { Title = "Error al eliminar el producto del carrito." }
                );

            return Ok();
        }

        /// <summary>
        /// Clear the basket.
        /// </summary>
        /// <param name="buyerId">
        /// The id of the buyer.
        /// </param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        private async Task<Basket?> RetrieveBasketAsync(string? buyerId)
        {
            if (string.IsNullOrWhiteSpace(buyerId))
            {
                Response.Cookies.Delete("buyerId");
                return null;
            }

            return await _dataContext
                .Baskets.Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.BuyerId == buyerId);
        }

        /// <summary>
        /// Get the buyer id.
        /// </summary>
        /// <returns>
        /// The buyer id.
        /// </returns>
        private string? GetBuyerId()
        {
            if (User.Identity is not null)
                return User.Identity.Name;
            else if (Request.Cookies.ContainsKey("buyerId"))
                return Request.Cookies["buyerId"];
            else
                return null;
        }

        /// <summary>
        /// Map a basket to a dto.
        /// </summary>
        /// <param name="basket">
        /// The basket to map.
        /// </param>
        /// <returns>
        /// The dto.
        /// </returns>
        private static BasketDto MapBasketToDto(Basket basket)
        {
            return new BasketDto
            {
                Id = basket.Id,
                BuyerId = basket.BuyerId,
                Items = basket
                    .Items.Select(item => new BasketItemDto
                    {
                        ProductId = item.ProductId,
                        Name = item.Product.Name,
                        Price = item.Product.Price,
                        ImageUrl = item.Product.ImageUrl,
                        Type = item.Product.Type,
                        Brand = item.Product.Brand,
                        Quantity = item.Quantity
                    })
                    .ToList()
            };
        }

        /// <summary>
        /// Create a basket.
        /// </summary>
        /// <returns>
        /// The basket.
        /// </returns>
        private async Task<Basket> CreateBasketAsync()
        {
            var buyerId = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(buyerId))
            {
                buyerId = Guid.NewGuid().ToString();
                var cookieOptions = new CookieOptions
                {
                    IsEssential = true,
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Append("buyerId", buyerId, cookieOptions);
            }

            var basket = new Basket { BuyerId = buyerId };
            await _dataContext.Baskets.AddAsync(basket);
            return basket;
        }
    }
}
