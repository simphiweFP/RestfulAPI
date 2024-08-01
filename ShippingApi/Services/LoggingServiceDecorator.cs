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

        public object GetOrderById(int id)
        {
            throw new NotImplementedException();
        }

        public object GetOrders()
        {
            throw new NotImplementedException();
        }

        public void AddOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrder(int id)
        {
            throw new NotImplementedException();
        }
    }
}
