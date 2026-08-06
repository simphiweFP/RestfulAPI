using ShippingApi.Models;
using ShippingApi.Dtos.Address;

namespace ShippingApi.Core
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        Task<Address?> GetDriverByAddress(string city, CancellationToken cancellationToken = default);
        Task<PagedResult<Address>> GetAddressesAsync(AddressQueryParameters queryParameters, CancellationToken cancellationToken = default);
    }
}
