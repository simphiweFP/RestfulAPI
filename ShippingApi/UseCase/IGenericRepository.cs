namespace ShippingApi.Core
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> All(CancellationToken cancellationToken = default);
        Task<T?> FindById(int id, CancellationToken cancellationToken = default);
        Task<bool> Add(T entity, CancellationToken cancellationToken = default);
        Task<bool> Update(T entity, CancellationToken cancellationToken = default);
        Task<bool> Delete(T entity, CancellationToken cancellationToken = default);
    }
}
