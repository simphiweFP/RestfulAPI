using Microsoft.EntityFrameworkCore;
using ShippingApi.Data;
using ShippingApi.Models;

namespace ShippingApi.UseCase.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .ToListAsync(cancellationToken);
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        }

        public async Task AddOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteOrderAsync(int orderId, CancellationToken cancellationToken = default)
        {
            var order = await _context.Orders.FindAsync(new object[] { orderId }, cancellationToken);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
