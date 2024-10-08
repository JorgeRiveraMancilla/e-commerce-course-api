using e_commerce_course_api.DTOs.Orders;

namespace e_commerce_course_api.Interfaces
{
    /// <summary>
    /// The order repository.
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// Create an order.
        /// </summary>
        /// <param name="orderDto">
        /// The order data transfer object.
        /// </param>
        /// <param name="userId">
        /// The user identifier.
        /// </param>
        /// <returns>
        /// The order data transfer object.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the user is not found.
        /// </exception>
        Task<OrderDto> CreateOrderAsync(OrderDto orderDto, int userId);

        /// <summary>
        /// Get the last order by user identifier.
        /// </summary>
        /// <param name="userId">
        /// The user identifier.
        /// </param>
        /// <returns>
        /// The order data transfer object.
        /// </returns>
        Task<OrderDto> GetLastOrderAsync(int userId);

        /// <summary>
        /// Get an order by its identifier.
        /// </summary>
        /// <param name="id">
        /// The order identifier.
        /// </param>
        /// <param name="userId">
        /// The user identifier.
        /// </param>
        /// <returns>
        /// The order data transfer object.
        /// </returns>
        Task<OrderDto> GetOrderByIdAsync(int id, int userId);

        /// <summary>
        /// Get orders by buyer identifier.
        /// </summary>
        /// <param name="userId">
        /// The user identifier.
        /// </param>
        /// <returns>
        /// The list of order data transfer objects.
        /// </returns>
        Task<List<OrderDto>> GetOrdersAsync(int userId);

        /// <summary>
        /// Get order status by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The order status data transfer object.
        /// </returns>
        Task<OrderStatusDto?> GetOrderStatusByNameAsync(string name);

        /// <summary>
        /// Save changes.
        /// </summary>
        /// <returns>
        /// A boolean indicating if the changes were saved.
        /// </returns>
        Task<bool> SaveChangesAsync();

        /// <summary>
        /// Update the order status.
        /// </summary>
        /// <param name="paymentIntentId">
        /// The payment intent identifier.
        /// </param>
        /// <param name="orderStatusDto">
        /// The order status data transfer object.
        /// </param>
        /// <returns>
        /// The task.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the order is not found.
        /// </exception>
        Task UpdateOrderStatusAsync(string paymentIntentId, OrderStatusDto orderStatusDto);
    }
}
