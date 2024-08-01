using ShippingApi.Models;

namespace ShippingApi.Services
{
    public interface IOrderService
    {
        void PlaceOrder(int userId, IEnumerable<Item> items);
        IEnumerable<Order> GetUserOrders(int userId);
        object GetOrderById(int id);
        object GetOrders();
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(int id);
    }
}
