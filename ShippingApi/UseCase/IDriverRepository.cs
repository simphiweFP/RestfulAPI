using ShippingApi.Core.Repository;
using ShippingApi.Models;

namespace ShippingApi.Core
{
    public interface IDriverRepository : IGenericRepository<Driver>
    {
        Task<Driver?> GetDriverByNumber(int driverNumber);
    }
}
