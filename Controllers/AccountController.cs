using e_commerce_course_api.DTOs;
using e_commerce_course_api.Entities;
using e_commerce_course_api.Extensions;
using e_commerce_course_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_course_api.Controllers
{
    /// <summary>
    /// The account controller.
    /// </summary>
    /// <param name="userManager">
    /// The user manager.
    /// </param>
    /// <param name="tokenService">
    /// The token service.
    /// </param>
    /// <param name="basketRepository">
    /// The basket repository.
    /// </param>
    public class AccountController(
        UserManager<User> userManager,
        ITokenService tokenService,
        IBasketRepository basketRepository
    ) : BaseApiController
    {
        /// <summary>
        /// The user manager.
        /// </summary>
        private readonly UserManager<User> _userManager = userManager;

        /// <summary>
        /// The token service.
        /// </summary>
        private readonly ITokenService _tokenService = tokenService;

        /// <summary>
        /// The basket repository.
        /// </summary>
        private readonly IBasketRepository _basketRepository = basketRepository;

        /// <summary>
        /// Registers a user.
        /// </summary>
        /// <param name="registerDto">
        /// The register data transfer object.
        /// </param>
        /// <returns>
        /// Status code 201 if the registration is successful; otherwise, a validation problem.
        /// </returns>
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var user = new User { UserName = registerDto.Name, Email = registerDto.Email, };
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description);
                return ValidationProblem();
            }

            result = await _userManager.AddToRoleAsync(user, "Member");

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description);
                return ValidationProblem();
            }

            return StatusCode(201);
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="loginDto">
        /// The login data transfer object.
        /// </param>
        /// <returns>
        /// The authentication data transfer object if the login is successful; otherwise, an unauthorized response.
        /// </returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (
                user is null
                || user.UserName is null
                || user.Email is null
                || !await _userManager.CheckPasswordAsync(user, loginDto.Password)
            )
                return Unauthorized();

            var buyerId = Request.Cookies["buyerId"];
            var userBasket = await _basketRepository.GetBasketByBuyerIdAsync(user.Id.ToString());
            var anonymousBasket = buyerId is not null
                ? await _basketRepository.GetBasketByBuyerIdAsync(buyerId)
                : null;

            if (userBasket is null)
                if (anonymousBasket is null)
                    userBasket = await _basketRepository.CreateBasketAsync(user.Id.ToString());
                else
                {
                    await _basketRepository.UpdateBuyerIdAsync(buyerId!, user.Id.ToString());
                    userBasket = anonymousBasket;
                    userBasket.BuyerId = user.Id.ToString();
                }
            else if (anonymousBasket is not null)
                await _basketRepository.RemoveBasketAsync(anonymousBasket.Id);

            Response.Cookies.Delete("buyerId");

            return new AuthDto
            {
                Name = user.UserName,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user),
                Token = await _tokenService.GenerateTokenAsync(user),
                Basket = userBasket,
            };
        }

        [Authorize]
        [HttpGet("current-user")]
        public async Task<ActionResult<AuthDto>> GetCurrentUser()
        {
            var userId = User.GetUserId().ToString();

            if (userId is null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null || user.UserName is null || user.Email is null)
                return Unauthorized();

            var userBasket =
                await _basketRepository.GetBasketByBuyerIdAsync(user.Id.ToString())
                ?? await _basketRepository.CreateBasketAsync(user.Id.ToString());

            return new AuthDto
            {
                Name = user.UserName,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user),
                Token = await _tokenService.GenerateTokenAsync(user),
                Basket = userBasket,
            };
        }
    }
}
