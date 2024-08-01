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

            public override async Task<IEnumerable<Driver>> All()
            {
                try
                {
                    return await _context.Drivers.Where(x => x.Id < 100).ToListAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            public async Task<Driver?> GetDriverByNumber(int driverNumber)
            {
                try
                {
                    return await _context.Drivers.FirstOrDefaultAsync(x => x.DriverNumber == driverNumber);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            public override async Task<Driver?> FindById(int id)
            {
                try
                {
                    return await _context.Drivers
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id);

                }
                catch (Exception e)
                {
                    Console.WriteLine();
                    throw;
                }
            }
        }
    }

