using ShippingApi.Models;

namespace ShippingApi.Services
{
    public interface IOrderService
    {
        void PlaceOrder(int userId, IEnumerable<Item> items);
        IEnumerable<Order> GetUserOrders(int userId);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
    }
}
