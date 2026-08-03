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

        public void PlaceOrder(int userId, IEnumerable<Item> items)
        {
            Console.WriteLine($"Order placed by user with ID {userId}");
            _orderService.PlaceOrder(userId, items);
        }

        public IEnumerable<Order> GetUserOrders(int userId)
        {
            Console.WriteLine($"Retrieving orders for user with ID {userId}");
            return _orderService.GetUserOrders(userId);
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            Console.WriteLine($"Retrieving order with ID {id}");
            return await _orderService.GetOrderByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            Console.WriteLine("Retrieving all orders");
            return await _orderService.GetOrdersAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            Console.WriteLine($"Adding order for user with ID {order.UserId}");
            await _orderService.AddOrderAsync(order);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            Console.WriteLine($"Updating order with ID {order.Id}");
            await _orderService.UpdateOrderAsync(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            Console.WriteLine($"Deleting order with ID {id}");
            await _orderService.DeleteOrderAsync(id);
        }
    }
}
