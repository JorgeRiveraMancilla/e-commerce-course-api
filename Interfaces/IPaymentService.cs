using e_commerce_course_api.DTOs.Baskets;
using Stripe;

namespace e_commerce_course_api.Interfaces
{
    /// <summary>
    /// The payment service.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Creates or updates the payment intent.
        /// </summary>
        /// <param name="basketDto">
        /// The basket dto.
        /// </param>
        /// <returns>
        /// The payment intent.
        /// </returns>
        Task<PaymentIntent> CreateOrUpdatePaymentIntent(BasketDto basketDto);
    }
}
