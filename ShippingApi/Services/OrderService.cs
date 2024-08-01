using Microsoft.EntityFrameworkCore;
using ShippingApi.Models;
using ShippingApi.Services;
using ShippingApi.UseCase;

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

        _orderRepository.AddOrder(order);
    }

    public IEnumerable<Order> GetUserOrders(int userId)
    {
       return _orderRepository.GetOrders().Where(o => o.UserId == userId);
    }

    public object GetOrderById(int id)
    {
        return _orderRepository.GetOrders().Where(o => o.Id== id);

    }

    public object GetOrders()
    {
        return _orderRepository.GetOrders().ToList();
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
