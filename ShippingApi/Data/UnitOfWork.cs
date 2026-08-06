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
            var logger = loggerFactory.CreateLogger(categoryName: "logs");
            Drivers = new DriverRepository(_context, logger);
            Address = new AddressRepository(_context, logger);
        }

        public async Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task RunMigrationsAsync(CancellationToken cancellationToken = default)
        {
             await _context.Database.MigrateAsync(cancellationToken);
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
