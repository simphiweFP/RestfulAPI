using Microsoft.EntityFrameworkCore;
using ShippingApi.Data;
using ShippingApi.Dtos.Order;
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

        public async Task<PagedResult<Order>> GetOrdersAsync(OrderQueryParameters queryParameters, CancellationToken cancellationToken = default)
        {
            var query = _context.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .AsQueryable();

            if (queryParameters.UserId.HasValue)
            {
                query = query.Where(o => o.UserId == queryParameters.UserId.Value);
            }

            if (queryParameters.MinTotalAmount.HasValue)
            {
                query = query.Where(o => o.TotalAmount >= queryParameters.MinTotalAmount.Value);
            }

            if (queryParameters.MaxTotalAmount.HasValue)
            {
                query = query.Where(o => o.TotalAmount <= queryParameters.MaxTotalAmount.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalCount / (double)queryParameters.PageSize);

            var items = await query
                .OrderByDescending(o => o.Id)
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Order>(items, totalCount, queryParameters.PageNumber, queryParameters.PageSize, totalPages);
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
            var existingOrder = await _context.Orders
                .Include(existing => existing.Items)
                .FirstOrDefaultAsync(existing => existing.Id == order.Id, cancellationToken);

            if (existingOrder is null)
            {
                return;
            }

            existingOrder.UserId = order.UserId;
            existingOrder.TotalAmount = order.TotalAmount;
            _context.Items.RemoveRange(existingOrder.Items);
            existingOrder.Items = order.Items.ToList();
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
