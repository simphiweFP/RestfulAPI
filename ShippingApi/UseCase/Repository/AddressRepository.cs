using Microsoft.EntityFrameworkCore;
using ShippingApi.Data;
using ShippingApi.Models;
using ShippingApi.Dtos.Address;

namespace ShippingApi.Core.Repository
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(ApplicationDbContext context, ILogger logger) : base(context,logger) 
        { 
        }
        public override async Task<IEnumerable<Address>> All(CancellationToken cancellationToken = default)
        {
            return await _context.Address
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Address?> GetDriverByAddress(string city, CancellationToken cancellationToken = default)
        {
            return await _context.Address
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.City == city, cancellationToken);
        }

        public async Task<PagedResult<Address>> GetAddressesAsync(AddressQueryParameters queryParameters, CancellationToken cancellationToken = default)
        {
            var query = _context.Address.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParameters.City))
            {
                query = query.Where(address => address.City == queryParameters.City);
            }

            if (!string.IsNullOrWhiteSpace(queryParameters.Search))
            {
                query = query.Where(address => address.Street.Contains(queryParameters.Search));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalCount / (double)queryParameters.PageSize);
            var items = await query
                .OrderBy(address => address.Id)
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Address>(items, totalCount, queryParameters.PageNumber, queryParameters.PageSize, totalPages);
        }
    }
}
