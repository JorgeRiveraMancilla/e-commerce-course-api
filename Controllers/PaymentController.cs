using e_commerce_course_api.DTOs.Baskets;
using e_commerce_course_api.Extensions;
using e_commerce_course_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace e_commerce_course_api.Controllers
{
    public class PaymentController(
        IConfiguration config,
        IBasketRepository basketRepository,
        IOrderRepository orderRepository,
        IPaymentService paymentService
    ) : BaseApiController
    {
        private readonly IConfiguration _config = config;
        private readonly IBasketRepository _basketRepository = basketRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IPaymentService _paymentService = paymentService;

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent()
        {
            var userId = User.GetUserId();

            var basket = await _basketRepository.GetBasketByBuyerIdAsync(userId.ToString());

            if (basket is null)
                return NotFound();

            var intent = await _paymentService.CreateOrUpdatePaymentIntent(basket);

            if (intent is null)
                return BadRequest(
                    new ProblemDetails { Title = "Problema al crear el intento de pago." }
                );

            if (intent.Id is not null && intent.ClientSecret is not null)
            {
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
                await _basketRepository.UpdateBasketAsync(basket);

                if (!await _basketRepository.SaveChangesAsync())
                    return BadRequest(
                        new ProblemDetails { Title = "Problema al guardar el intento de pago." }
                    );
            }

            return Ok(basket);
        }

        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                _config["StripeSettings:WhSecret"]
            );

            string? paymentIntentId;
            bool isPaymentSuccessful;

            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentSucceeded:
                    var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    paymentIntentId = paymentIntent.Id;
                    isPaymentSuccessful = paymentIntent.Status == "succeeded";
                    break;

                case Events.ChargeSucceeded:
                    var charge = (Charge)stripeEvent.Data.Object;
                    paymentIntentId = charge.PaymentIntentId;
                    isPaymentSuccessful = charge.Status == "succeeded";
                    break;

                case Events.PaymentIntentCreated:
                case Events.ChargeUpdated:
                    return new EmptyResult();

                default:
                    return new EmptyResult();
            }

            if (isPaymentSuccessful && !string.IsNullOrEmpty(paymentIntentId))
            {
                var orderStatus = await _orderRepository.GetOrderStatusByNameAsync(
                    "Payment Received"
                );

                if (orderStatus is null)
                {
                    return BadRequest(
                        new ProblemDetails { Title = "Problema al obtener el estado del pedido." }
                    );
                }

                try
                {
                    await _orderRepository.UpdateOrderStatusAsync(paymentIntentId, orderStatus);

                    if (!await _orderRepository.SaveChangesAsync())
                    {
                        return BadRequest(
                            new ProblemDetails
                            {
                                Title = "Problema al guardar el estado del pedido.",
                            }
                        );
                    }
                }
                catch (Exception)
                {
                    return new EmptyResult();
                }
            }

            return new EmptyResult();
        }
    }
}
