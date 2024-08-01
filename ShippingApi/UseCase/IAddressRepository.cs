using ShippingApi.Models;

namespace ShippingApi.Core
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        Task<Address?> GetDriverByAddress(string city);
    }
}
