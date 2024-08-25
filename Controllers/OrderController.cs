using AutoMapper;
using e_commerce_course_api.DTOs.Orders;
using e_commerce_course_api.Extensions;
using e_commerce_course_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_course_api.Controllers
{
    /// <summary>
    /// The order controller.
    /// </summary>
    /// <param name="basketRepository">
    /// The repository for the basket.
    /// </param>
    /// <param name="mapper">
    /// The mapper to use.
    /// </param>
    /// <param name="orderRepository">
    /// The repository for the orders.
    /// </param>
    /// <param name="productRepository">
    /// The repository for the products.
    /// </param>
    /// <param name="userRepository">
    /// The repository for the users.
    /// </param>
    [Authorize]
    public class OrderController(
        IBasketRepository basketRepository,
        IMapper mapper,
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IUserRepository userRepository
    ) : BaseApiController
    {
        /// <summary>
        /// The basket repository to use.
        /// </summary>
        private readonly IBasketRepository _basketRepository = basketRepository;

        /// <summary>
        /// The mapper to use.
        /// </summary>
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// The order repository to use.
        /// </summary>
        private readonly IOrderRepository _orderRepository = orderRepository;

        /// <summary>
        /// The product repository to use.
        /// </summary>
        private readonly IProductRepository _productRepository = productRepository;

        /// <summary>
        /// The user repository to use.
        /// </summary>
        private readonly IUserRepository _userRepository = userRepository;

        /// <summary>
        /// Get the orders.
        /// </summary>
        /// <returns>
        /// The orders.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetOrders()
        {
            var userId = User.GetUserId();
            var orders = await _orderRepository.GetOrdersAsync(userId);
            return Ok(orders);
        }

        /// <summary>
        /// Get an order by id.
        /// </summary>
        /// <param name="id">
        /// The id of the order.
        /// </param>
        /// <returns>
        /// The order if exists, otherwise not found.
        /// </returns>
        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var userId = User.GetUserId();
            var order = await _orderRepository.GetOrderByIdAsync(id, userId);
            return Ok(order);
        }

        /// <summary>
        /// Create an order.
        /// </summary>
        /// <param name="createOrderDto">
        /// The order to create.
        /// </param>
        /// <returns>
        /// The order if created, otherwise bad request.
        /// </returns>

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto createOrderDto)
        {
            var userId = User.GetUserId();

            var basket = await _basketRepository.GetBasketByBuyerIdAsync(userId.ToString());

            if (basket is null)
                return BadRequest(new ProblemDetails { Title = "Carrito no encontrado." });

            var orderItems = new List<OrderItemDto>();

            foreach (var item in basket.Items)
            {
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);

                if (product is null)
                    return BadRequest(new ProblemDetails { Title = "Producto no encontrado." });

                var orderItem = _mapper.Map<OrderItemDto>(
                    product,
                    opt =>
                        opt.AfterMap(
                            (src, dest) =>
                            {
                                dest.Quantity = item.Quantity;
                            }
                        )
                );

                orderItems.Add(orderItem);

                await _productRepository.UpdateStockAsync(product.Id, -item.Quantity);

                if (!await _productRepository.SaveChangesAsync())
                    return BadRequest(
                        new ProblemDetails { Title = "Error al actualizar el stock." }
                    );
            }

            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);
            var deliveryFee = 100000 < subtotal ? 0 : 5000;

            var order = new OrderDto
            {
                OrderDate = DateTime.UtcNow,
                Subtotal = subtotal,
                DeliveryFee = deliveryFee,
                Total = subtotal + deliveryFee,
                OrderStatus = new OrderStatusDto { Name = "Pending" },
                OrderItems = orderItems,
                Address = createOrderDto.Address
            };

            order = await _orderRepository.CreateOrderAsync(order, userId);

            if (!await _productRepository.SaveChangesAsync())
                return BadRequest(new ProblemDetails { Title = "Error al crear la orden." });

            await _basketRepository.RemoveBasketAsync(basket.Id);

            if (!await _basketRepository.SaveChangesAsync())
                return BadRequest(new ProblemDetails { Title = "Error al eliminar el carrito." });

            if (createOrderDto.SaveAddress)
            {
                var address = createOrderDto.Address;

                var result = await _userRepository.UpdateAddressAsync(address, userId);

                if (!result.Succeeded)
                    return BadRequest(
                        new ProblemDetails { Title = "Error al guardar la dirección." }
                    );
            }

            order = await _orderRepository.GetLastOrderAsync(userId);

            return CreatedAtRoute("GetOrder", new { id = order.Id }, order.Id);
        }
    }
}
