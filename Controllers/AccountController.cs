using e_commerce_course_api.Data;
using e_commerce_course_api.DTOs;
using e_commerce_course_api.Entities;
using e_commerce_course_api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_course_api.Controllers
{
    public class AccountController(
        UserManager<User> userManager,
        ITokenService tokenService,
        DataContext dataContext
    ) : BaseApiController
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly ITokenService _tokenService = tokenService;
        private readonly DataContext _dataContext = dataContext;

        /// <summary>
        /// Register a user.
        /// </summary>
        /// <param name="registerDto">
        /// The data to use for the registration.
        /// </param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var user = new User { UserName = registerDto.Username, Email = registerDto.Email };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }

            await _userManager.AddToRoleAsync(user, "Member");

            return StatusCode(201);
        }

        /// <summary>
        /// Login a user.
        /// </summary>
        /// <param name="loginDto">
        /// The data to use for the login.
        /// </param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Unauthorized();

            var userBasket = await RetrieveBasketAsync(loginDto.Email);
            var anonBasket = await RetrieveBasketAsync(Request.Cookies["buyerId"]);

            // FIXME: Change the buyerId to the user's email in the rest of the code.
            if (anonBasket != null)
            {
                if (userBasket != null)
                    _dataContext.Baskets.Remove(userBasket);
                anonBasket.BuyerId = user.Email;
                Response.Cookies.Delete("buyerId");
                await _dataContext.SaveChangesAsync();
            }

            return new UserDto
            {
                Email = loginDto.Email,
                Token = await _tokenService.GenerateTokenAsync(user),
                Basket =
                    anonBasket != null ? MapBasketToDto(anonBasket) : MapBasketToDto(userBasket)
            };
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
    }
}
