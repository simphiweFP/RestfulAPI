using ShippingApi.Models;

namespace ShippingApi.UseCase
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrdersAsync(CancellationToken cancellationToken = default);
        Task<Order?> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken = default);
        Task AddOrderAsync(Order order, CancellationToken cancellationToken = default);
        Task UpdateOrderAsync(Order order, CancellationToken cancellationToken = default);
        Task DeleteOrderAsync(int orderId, CancellationToken cancellationToken = default);
    }
}
