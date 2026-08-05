using Microsoft.EntityFrameworkCore;
using ShippingApi.Data;
using ShippingApi.Models;

namespace ShippingApi.Core.Repository
{
        public class DriverRepository : GenericRepository<Driver>, IDriverRepository
        {
            public DriverRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
            {
            }

            public override async Task<IEnumerable<Driver>> All(CancellationToken cancellationToken = default)
            {
                try
                {
                    return await _context.Drivers.Where(x => x.Id < 100).ToListAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            public async Task<Driver?> GetDriverByNumber(int driverNumber, CancellationToken cancellationToken = default)
            {
                try
                {
                    return await _context.Drivers.FirstOrDefaultAsync(x => x.DriverNumber == driverNumber, cancellationToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            public override async Task<Driver?> FindById(int id, CancellationToken cancellationToken = default)
            {
                try
                {
                    return await _context.Drivers
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine();
                    throw;
                }
            }
        }
    }

