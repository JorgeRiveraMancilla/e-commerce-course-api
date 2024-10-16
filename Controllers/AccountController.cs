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
    /// <param name="addressRepository">
    /// The address repository.
    /// </param>
    /// <param name="basketRepository">
    /// The basket repository.
    /// </param>
    /// <param name="userRepository">
    /// The user repository.
    /// </param>
    public class AccountController(
        IAddressRepository addressRepository,
        IBasketRepository basketRepository,
        IUserRepository userRepository
    ) : BaseApiController
    {
        /// <summary>
        /// The address repository.
        /// </summary>
        private readonly IAddressRepository _addressRepository = addressRepository;

        /// <summary>
        /// The basket repository.
        /// </summary>
        private readonly IBasketRepository _basketRepository = basketRepository;

        /// <summary>
        /// The user repository.
        /// </summary>
        private readonly IUserRepository _userRepository = userRepository;

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
            var result = await _userRepository.CreateUserAsync(registerDto);

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
            var user = await _userRepository.LoginAsync(loginDto);

            if (user is null)
                return Unauthorized();

            var buyerId = Request.Cookies["buyerId"];

            var userBasket = await _basketRepository.GetBasketByBuyerIdAsync(user.Id.ToString());
            var anonymousBasket = buyerId is null
                ? null
                : await _basketRepository.GetBasketByBuyerIdAsync(buyerId);

            switch (userBasket, anonymousBasket)
            {
                case (null, null):
                    userBasket = await _basketRepository.CreateBasketAsync(user.Id.ToString());

                    if (!await _basketRepository.SaveChangesAsync())
                        return BadRequest("Intente de nuevo.");

                    break;
                case (null, not null):
                    anonymousBasket.BuyerId = user.Id.ToString();
                    await _basketRepository.UpdateBasketAsync(anonymousBasket);

                    if (!await _basketRepository.SaveChangesAsync())
                        return BadRequest("Intente de nuevo.");

                    userBasket = anonymousBasket;
                    break;
                case (not null, null):
                    break;
                case (not null, not null):
                    await _basketRepository.RemoveBasketAsync(anonymousBasket.Id);

                    if (!await _basketRepository.SaveChangesAsync())
                        return BadRequest("Intente de nuevo.");

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
            int userId = User.GetUserId();

            var userBasket =
                await _basketRepository.GetBasketByBuyerIdAsync(userId.ToString())
                ?? await _basketRepository.CreateBasketAsync(userId.ToString());

            return Ok(await _userRepository.GetAuthByIdAsync(userId, userBasket));
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
            var userId = User.GetUserId();

            var address = await _addressRepository.GetAddressByIdAsync(userId);

            if (address is not null)
                address.Id = 0;

            return Ok(address);
        }
    }
}
