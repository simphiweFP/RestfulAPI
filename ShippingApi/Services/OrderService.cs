using ShippingApi.Models;
using ShippingApi.Services;
using ShippingApi.UseCase;

namespace ShippingApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void PlaceOrder(int userId, IEnumerable<Item> items)
        {
            var order = new Order
            {
                UserId = userId,
                Items = items.ToList(),
                TotalAmount = items.Sum(i => i.Price)
            };

            _orderRepository.AddOrderAsync(order).Wait();
        }

        public IEnumerable<Order> GetUserOrders(int userId)
        {
            return _orderRepository.GetOrdersAsync().Result.Where(o => o.UserId == userId);
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetOrderByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _orderRepository.GetOrdersAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            await _orderRepository.AddOrderAsync(order);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            await _orderRepository.UpdateOrderAsync(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _orderRepository.DeleteOrderAsync(id);
        }
    }
}
