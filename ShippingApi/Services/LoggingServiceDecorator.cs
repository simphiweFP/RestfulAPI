using ShippingApi.Dtos.Order;
using ShippingApi.Models;

namespace ShippingApi.Services
{
    public class LoggingServiceDecorator : IOrderService
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<LoggingServiceDecorator> _logger;

        public LoggingServiceDecorator(IOrderService orderService, ILogger<LoggingServiceDecorator> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        public async Task PlaceOrderAsync(int userId, IEnumerable<Item> items, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Placing order for user {UserId} with {ItemCount} items", userId, items.Count());
            await _orderService.PlaceOrderAsync(userId, items, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving orders for user {UserId}", userId);
            return await _orderService.GetUserOrdersAsync(userId, cancellationToken);
        }

        public async Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving order {OrderId}", id);
            return await _orderService.GetOrderByIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving all orders");
            return await _orderService.GetOrdersAsync(cancellationToken);
        }

        public async Task<PagedResult<Order>> GetOrdersAsync(OrderQueryParameters queryParameters, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving orders page {PageNumber} with size {PageSize}", queryParameters.PageNumber, queryParameters.PageSize);
            return await _orderService.GetOrdersAsync(queryParameters, cancellationToken);
        }

        public async Task AddOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Adding order for user {UserId}", order.UserId);
            await _orderService.AddOrderAsync(order, cancellationToken);
        }

        public async Task UpdateOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Updating order {OrderId}", order.Id);
            await _orderService.UpdateOrderAsync(order, cancellationToken);
        }

        public async Task DeleteOrderAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Deleting order {OrderId}", id);
            await _orderService.DeleteOrderAsync(id, cancellationToken);
        }
    }
}
