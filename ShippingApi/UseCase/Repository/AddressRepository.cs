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
        public override async Task<IEnumerable<Address>> All()
        {
            try
            {
                return await _context.Address.Where(x => x.City == "Durban" || x.City == "Ballito" || x.City == "Pietermarizburg" || x.City == "Empangeni").ToListAsync();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public Task<Address?> GetDriverByAddress(string city)
        {
            throw new NotImplementedException();
        }
    }
}
