using Microsoft.EntityFrameworkCore;
using ShippingApi.Data;
using ShippingApi.Models;

namespace ShippingApi.Core.Repository
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(ApplicationDbContext context, ILogger logger) : base(context,logger) 
        { 
        }
        public override async Task<IEnumerable<Address>> All(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Address
                    .Where(x => x.City == "Durban" || x.City == "Ballito" || x.City == "Pietermarizburg" || x.City == "Empangeni")
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<Address?> GetDriverByAddress(string city, CancellationToken cancellationToken = default)
        {
            return await _context.Address.FirstOrDefaultAsync(x => x.City == city, cancellationToken);
        }
    }
}
