using e_commerce_course_api.DTOs.Baskets;
using e_commerce_course_api.Interfaces;
using Stripe;

namespace e_commerce_course_api.Services
{
    /// <summary>
    /// The payment service.
    /// </summary>
    /// <param name="config">
    /// The configuration.
    /// </param>
    public class PaymentService(IConfiguration config) : IPaymentService
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IConfiguration _config = config;

        /// <summary>
        /// Creates or updates the payment intent.
        /// </summary>
        /// <param name="basketDto">
        /// The basket dto.
        /// </param>
        /// <returns>
        /// The payment intent.
        /// </returns>
        public async Task<PaymentIntent> CreateOrUpdatePaymentIntent(BasketDto basketDto)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

            var service = new PaymentIntentService();
            var intent = new PaymentIntent();

            var subtotal = basketDto.Items.Sum(i => i.Quantity * i.Price);
            var deliveryFee = 100000 < subtotal ? 0 : 5000;

            if (string.IsNullOrEmpty(basketDto.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = subtotal + deliveryFee,
                    Currency = "usd",
                    PaymentMethodTypes = ["card"],
                };

                intent = await service.CreateAsync(options);
            }
            else
            {
                var options = new PaymentIntentUpdateOptions { Amount = subtotal + deliveryFee };
                intent = await service.UpdateAsync(basketDto.PaymentIntentId, options); // FIX: Asignar el resultado
            }

            return intent;
        }
    }
}
