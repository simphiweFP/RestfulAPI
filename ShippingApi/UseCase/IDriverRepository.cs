using ShippingApi.Core.Repository;
using ShippingApi.Models;
using ShippingApi.Dtos.Driver;

namespace ShippingApi.Core
{
    public interface IDriverRepository : IGenericRepository<Driver>
    {
        Task<Driver?> GetDriverByNumber(int driverNumber, CancellationToken cancellationToken = default);
        Task<PagedResult<Driver>> GetDriversAsync(DriverQueryParameters queryParameters, CancellationToken cancellationToken = default);
    }
}
