namespace ShippingApi.Core
{
    public interface IUnitOfWork
    {
        IDriverRepository Drivers { get; }
        IAddressRepository Address { get; }

        Task RunMigrationsAsync(CancellationToken cancellationToken = default);
        Task CompleteAsync(CancellationToken cancellationToken = default);
        void Dispose();
    }
}
