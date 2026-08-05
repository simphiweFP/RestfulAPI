using ShippingApi.Models;

namespace ShippingApi.Services
{
    public class LoggingServiceDecorator: IOrderService
   {
    private readonly IOrderService _orderService;

        public LoggingServiceDecorator(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task PlaceOrderAsync(int userId, IEnumerable<Item> items, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"Order placed by user with ID {userId}");
            await _orderService.PlaceOrderAsync(userId, items, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"Retrieving orders for user with ID {userId}");
            return await _orderService.GetUserOrdersAsync(userId, cancellationToken);
        }

        public async Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"Retrieving order with ID {id}");
            return await _orderService.GetOrderByIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Retrieving all orders");
            return await _orderService.GetOrdersAsync(cancellationToken);
        }

        public async Task AddOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"Adding order for user with ID {order.UserId}");
            await _orderService.AddOrderAsync(order, cancellationToken);
        }

        public async Task UpdateOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"Updating order with ID {order.Id}");
            await _orderService.UpdateOrderAsync(order, cancellationToken);
        }

        public async Task DeleteOrderAsync(int id, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"Deleting order with ID {id}");
            await _orderService.DeleteOrderAsync(id, cancellationToken);
        }
    }
}
