using AutoMapper;
using e_commerce_course_api.DTOs.Orders;
using e_commerce_course_api.Entities;
using e_commerce_course_api.Entities.Orders;
using e_commerce_course_api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_course_api.Data.Repositories
{
    /// <summary>
    /// The order repository.
    /// </summary>
    /// <param name="dataContext">
    /// The data context.
    /// </param>
    /// <param name="mapper">
    /// The mapper.
    /// </param>
    public class OrderRepository(
        DataContext dataContext,
        IMapper mapper,
        UserManager<User> userManager
    ) : IOrderRepository
    {
        /// <summary>
        /// The data context.
        /// </summary>
        private readonly DataContext _dataContext = dataContext;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// The user manager.
        /// </summary>
        private readonly UserManager<User> _userManager = userManager;

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
        public async Task<OrderDto> CreateOrderAsync(OrderDto orderDto, int userId)
        {
            var user =
                await _userManager.FindByIdAsync(userId.ToString())
                ?? throw new Exception("Usuario no encontrado.");

            var order = _mapper.Map<Order>(
                orderDto,
                opt =>
                    opt.AfterMap(
                        (src, dest) =>
                        {
                            dest.User = user;
                        }
                    )
            );

            _ = await _dataContext.Orders.AddAsync(order);
            return _mapper.Map<OrderDto>(order);
        }

        /// <summary>
        /// Get the last order by user identifier.
        /// </summary>
        /// <param name="userId">
        /// The user identifier.
        /// </param>
        /// <returns>
        /// The order data transfer object.
        /// </returns>
        public async Task<OrderDto> GetLastOrderAsync(int userId)
        {
            var order = await _dataContext
                .Orders.Include(x => x.Address)
                .Include(x => x.OrderItems)
                .Where(x => x.User.Id == userId)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            return _mapper.Map<OrderDto>(order);
        }

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
        public async Task<OrderDto> GetOrderByIdAsync(int id, int userId)
        {
            var order = await _dataContext
                .Orders.Include(x => x.Address)
                .Include(x => x.OrderItems)
                .Include(x => x.OrderStatus)
                .FirstOrDefaultAsync(x => x.Id == id && x.User.Id == userId);
            return _mapper.Map<OrderDto>(order);
        }

        /// <summary>
        /// Get orders by buyer identifier.
        /// </summary>
        /// <param name="userId">
        /// The user identifier.
        /// </param>
        /// <returns>
        /// The list of order data transfer objects.
        /// </returns>
        public async Task<List<OrderDto>> GetOrdersAsync(int userId)
        {
            var orders = await _dataContext
                .Orders.Include(x => x.Address)
                .Include(x => x.OrderItems)
                .Include(x => x.OrderStatus)
                .Where(x => x.User.Id == userId)
                .ToListAsync();

            return _mapper.Map<List<OrderDto>>(orders);
        }

        /// <summary>
        /// Get order status by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The order status data transfer object.
        /// </returns>
        public async Task<OrderStatusDto?> GetOrderStatusByNameAsync(string name)
        {
            var orderStatus = await _dataContext.OrderStatuses.FirstOrDefaultAsync(x =>
                x.Name == name
            );

            return _mapper.Map<OrderStatusDto>(orderStatus);
        }

        /// <summary>
        /// Save changes.
        /// </summary>
        /// <returns>
        /// A boolean indicating if the changes were saved.
        /// </returns>
        public async Task<bool> SaveChangesAsync()
        {
            return 0 < await _dataContext.SaveChangesAsync();
        }

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
        public async Task UpdateOrderStatusAsync(
            string paymentIntentId,
            OrderStatusDto orderStatusDto
        )
        {
            var order =
                await _dataContext.Orders.FirstOrDefaultAsync(x =>
                    x.PaymentIntentId == paymentIntentId
                ) ?? throw new Exception("Orden no encontrada.");

            order.OrderStatus = _mapper.Map<OrderStatus>(orderStatusDto);
        }
    }
}
