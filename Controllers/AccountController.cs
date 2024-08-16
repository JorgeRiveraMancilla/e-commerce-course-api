using e_commerce_course_api.DTOs;
using e_commerce_course_api.Extensions;
using e_commerce_course_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_course_api.Controllers
{
    /// <summary>
    /// The account controller.
    /// </summary>
    /// <param name="userRepository">
    /// The user repository.
    /// </param>
    /// <param name="basketRepository">
    /// The basket repository.
    /// </param>
    public class AccountController(
        IUserRepository userRepository,
        IBasketRepository basketRepository
    ) : BaseApiController
    {
        /// <summary>
        /// The user repository.
        /// </summary>
        private readonly IUserRepository _userRepository = userRepository;

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
            var result = await _userRepository.CreateUserAsync(
                registerDto.Name,
                registerDto.Email,
                registerDto.Password
            );

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
            if (!await _userRepository.CheckLogin(loginDto))
                return Unauthorized();

            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            var buyerId = Request.Cookies["buyerId"];

            var userBasket = await _basketRepository.GetBasketByBuyerIdAsync(user.Id.ToString());
            var anonymousBasket = buyerId is null
                ? null
                : await _basketRepository.GetBasketByBuyerIdAsync(buyerId);

            switch (userBasket, anonymousBasket)
            {
                case (null, null):
                    userBasket = await _basketRepository.CreateBasketAsync(user.Id.ToString());
                    break;
                case (null, not null):
                    userBasket = await _basketRepository.UpdateBuyerIdAsync(
                        buyerId!,
                        user.Id.ToString()
                    );
                    break;
                case (not null, null):
                    break;
                case (not null, not null):
                    await _basketRepository.RemoveBasketAsync(anonymousBasket.Id);
                    break;
            }

            Response.Cookies.Delete("buyerId");

            return Ok(await _userRepository.GetAuthByIdAsync(user.Id, userBasket));
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <returns>
        /// The authentication data transfer object.
        /// </returns>
        [Authorize]
        [HttpGet("current-user")]
        public async Task<ActionResult<AuthDto>> GetCurrentUser()
        {
            int id = (int)User.GetUserId()!;
            var user = await _userRepository.GetUserByIdAsync(id);

            var userBasket =
                await _basketRepository.GetBasketByBuyerIdAsync(user.Id.ToString())
                ?? await _basketRepository.CreateBasketAsync(user.Id.ToString());

            return Ok(await _userRepository.GetAuthByIdAsync(user.Id, userBasket));
        }

        /// <summary>
        /// Gets the saved address.
        /// </summary>
        /// <returns>
        /// The address data transfer object if found; otherwise, null.
        /// </returns>
        [Authorize]
        [HttpGet("saved-address")]
        public async Task<ActionResult<AddressDto?>> GetSavedAddress()
        {
            int id = (int)User.GetUserId()!;
            var user = await _userRepository.GetUserByIdAsync(id);

            return Ok(await _userRepository.GetAddressByIdAsync(user.Id));
        }
    }
}
