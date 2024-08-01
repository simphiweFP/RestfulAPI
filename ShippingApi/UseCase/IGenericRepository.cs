namespace ShippingApi.Core
{
    public interface IGenericRepository<T> where T : class //make generic
    {
        Task<IEnumerable<T>> All();
        Task<T?> FindById(int id);
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
    }
}
