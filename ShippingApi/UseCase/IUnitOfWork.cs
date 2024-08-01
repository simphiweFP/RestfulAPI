namespace ShippingApi.Core
{
    public interface IUnitOfWork
    {
        IDriverRepository Drivers { get; }
        IAddressRepository Address { get; }

        Task RunMigrationsAsync();
        Task CompleteAsync();
        void Dispose();
    }
}
