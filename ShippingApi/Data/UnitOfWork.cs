using ShippingApi.Core.Repository;
using ShippingApi.Core;
using Microsoft.EntityFrameworkCore;

namespace ShippingApi.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public IDriverRepository Drivers { get; private set; }
        public IAddressRepository Address { get; private set; }


        public UnitOfWork(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {

            _context = context;
            var _logger = loggerFactory.CreateLogger(categoryName: "logs");
            Drivers = new DriverRepository(_context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task RunMigrationsAsync()
        {
             await _context.Database.MigrateAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
