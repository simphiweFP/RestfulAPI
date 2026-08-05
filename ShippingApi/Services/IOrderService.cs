using ShippingApi.Models;

namespace ShippingApi.Services
{
    public interface IOrderService
    {
        Task PlaceOrderAsync(int userId, IEnumerable<Item> items, CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId, CancellationToken cancellationToken = default);
        Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> GetOrdersAsync(CancellationToken cancellationToken = default);
        Task AddOrderAsync(Order order, CancellationToken cancellationToken = default);
        Task UpdateOrderAsync(Order order, CancellationToken cancellationToken = default);
        Task DeleteOrderAsync(int id, CancellationToken cancellationToken = default);
    }
}
