using ShippingApi.Models;

namespace ShippingApi.UseCase
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrders();
        Order GetOrderById(int orderId);
        void AddOrder(Order order);
    }
}
