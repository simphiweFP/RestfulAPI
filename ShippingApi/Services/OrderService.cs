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

        public async Task PlaceOrderAsync(int userId, IEnumerable<Item> items, CancellationToken cancellationToken = default)
        {
            var order = new Order
            {
                UserId = userId,
                Items = items.ToList(),
                TotalAmount = items.Sum(i => i.Price)
            };

            await _orderRepository.AddOrderAsync(order, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId, CancellationToken cancellationToken = default)
        {
            var orders = await _orderRepository.GetOrdersAsync(cancellationToken);
            return orders.Where(o => o.UserId == userId);
        }

        public async Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _orderRepository.GetOrderByIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(CancellationToken cancellationToken = default)
        {
            return await _orderRepository.GetOrdersAsync(cancellationToken);
        }

        public async Task AddOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _orderRepository.AddOrderAsync(order, cancellationToken);
        }

        public async Task UpdateOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _orderRepository.UpdateOrderAsync(order, cancellationToken);
        }

        public async Task DeleteOrderAsync(int id, CancellationToken cancellationToken = default)
        {
            await _orderRepository.DeleteOrderAsync(id, cancellationToken);
        }
    }
}
