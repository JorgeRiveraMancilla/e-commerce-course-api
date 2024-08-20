using e_commerce_course_api.DTOs.Baskets;
using e_commerce_course_api.Extensions;
using e_commerce_course_api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_course_api.Controllers
{
    /// <summary>
    /// The controller for the basket.
    /// </summary>
    /// <param name="basketRepository">
    /// The repository for the basket.
    /// </param>
    public class BasketController(IBasketRepository basketRepository) : BaseApiController
    {
        /// <summary>
        /// The data context to use.
        /// </summary>
        private readonly IBasketRepository _basketRepository = basketRepository;

        /// <summary>
        /// Get the basket.
        /// </summary>
        /// <returns>
        /// If the basket is found, the basket; otherwise, not found.
        /// </returns>
        [HttpGet(Name = "GetBasket")]
        public async Task<ActionResult<BasketDto?>> GetBasket()
        {
            var buyerId = GetBuyerId();

            if (buyerId is null)
            {
                Response.Cookies.Delete("buyerId");
                return NotFound();
            }

            var basket = await _basketRepository.GetBasketByBuyerIdAsync(buyerId);

            return Ok(basket);
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
        /// The route to the basket if the item is added; otherwise, bad request.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> AddItemToBasket(
            [FromQuery] int productId,
            [FromQuery] int quantity
        )
        {
            var buyerId = GetBuyerId();

            if (buyerId is null)
            {
                buyerId = Guid.NewGuid().ToString();
                var cookieOptions = new CookieOptions
                {
                    IsEssential = true,
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Append("buyerId", buyerId, cookieOptions);
            }

            _ =
                await _basketRepository.GetBasketByBuyerIdAsync(buyerId)
                ?? await _basketRepository.CreateBasketAsync(buyerId);
            await _basketRepository.SaveChangesAsync();

            BasketDto? basket;
            try
            {
                basket = await _basketRepository.AddItemToBasketAsync(buyerId, productId, quantity);
            }
            catch (Exception e)
            {
                return BadRequest(new ProblemDetails { Title = e.Message });
            }

            if (!await _basketRepository.SaveChangesAsync())
                return BadRequest(
                    new ProblemDetails { Title = "Error al añadir el producto al carrito." }
                );

            return CreatedAtRoute("GetBasket", basket);
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
        /// Ok if item is removed, not found if basket does not exist, bad request otherwise.
        /// </returns>
        [HttpDelete]
        public async Task<ActionResult> RemoveBasketItem(int productId, int quantity)
        {
            var buyerId = GetBuyerId();

            if (buyerId is null)
            {
                Response.Cookies.Delete("buyerId");
                return NotFound();
            }

            var basket = await _basketRepository.GetBasketByBuyerIdAsync(buyerId);

            if (basket is null)
                return NotFound();

            try
            {
                await _basketRepository.RemoveItemFromBasketAsync(basket.Id, productId, quantity);
            }
            catch (Exception e)
            {
                return BadRequest(new ProblemDetails { Title = e.Message });
            }

            if (!await _basketRepository.SaveChangesAsync())
                return BadRequest(
                    new ProblemDetails { Title = "Error al eliminar el producto del carrito." }
                );

            return Ok();
        }

        /// <summary>
        /// Get the buyer id.
        /// </summary>
        /// <returns>
        /// The buyer id.
        /// </returns>
        private string? GetBuyerId()
        {
            try
            {
                return User.GetUserId().ToString();
            }
            catch
            {
                return Request.Cookies["buyerId"];
            }
        }
    }
}
