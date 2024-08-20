namespace e_commerce_course_api.Entities.Orders
{
    /// <summary>
    /// Represents the status of an order.
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// The order is pending.
        /// </summary>
        Pending,

        /// <summary>
        /// The payment has been received.
        /// </summary>
        PaymentReceived,

        /// <summary>
        /// The payment has failed.
        /// </summary>
        PaymentFailed,
    }
}
