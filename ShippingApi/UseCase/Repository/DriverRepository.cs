using Microsoft.EntityFrameworkCore;
using ShippingApi.Data;
using ShippingApi.Models;
using ShippingApi.Dtos.Driver;

namespace ShippingApi.Core.Repository
{
        public class DriverRepository : GenericRepository<Driver>, IDriverRepository
        {
            public DriverRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
            {
            }

            public override async Task<IEnumerable<Driver>> All(CancellationToken cancellationToken = default)
            {
                return await _context.Drivers
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }

            public async Task<Driver?> GetDriverByNumber(int driverNumber, CancellationToken cancellationToken = default)
            {
                return await _context.Drivers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.DriverNumber == driverNumber, cancellationToken);
            }

            public override async Task<Driver?> FindById(int id, CancellationToken cancellationToken = default)
            {
                return await _context.Drivers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            }

            public async Task<PagedResult<Driver>> GetDriversAsync(DriverQueryParameters queryParameters, CancellationToken cancellationToken = default)
            {
                var query = _context.Drivers
                    .AsNoTracking()
                    .Include(driver => driver.Address)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(queryParameters.Team))
                {
                    query = query.Where(driver => driver.Team == queryParameters.Team);
                }

                if (!string.IsNullOrWhiteSpace(queryParameters.Search))
                {
                    query = query.Where(driver => driver.Name.Contains(queryParameters.Search) || driver.Email.Contains(queryParameters.Search));
                }

                var totalCount = await query.CountAsync(cancellationToken);
                var totalPages = (int)Math.Ceiling(totalCount / (double)queryParameters.PageSize);
                var items = await query
                    .OrderBy(driver => driver.Id)
                    .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                    .Take(queryParameters.PageSize)
                    .ToListAsync(cancellationToken);

                return new PagedResult<Driver>(items, totalCount, queryParameters.PageNumber, queryParameters.PageSize, totalPages);
            }
        }
    }

