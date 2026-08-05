using Microsoft.EntityFrameworkCore;
using ShippingApi.Data;

namespace ShippingApi.Core.Repository
{
        public class GenericRepository<T> : IGenericRepository<T> where T : class //inherete IGeneric
        {

            protected ApplicationDbContext _context; //fundation which database will be connected to
            internal DbSet<T> _dbSet;
            protected readonly ILogger _logger;

            public GenericRepository(ApplicationDbContext context, ILogger logger)
            {
                _context = context;
                _logger = logger;
                this._dbSet = _context.Set<T>();

            }

            public virtual async Task<bool> Add(T entity, CancellationToken cancellationToken = default)
            {
                await _dbSet.AddAsync(entity, cancellationToken);
                return true;
            }

            public virtual async Task<IEnumerable<T>> All(CancellationToken cancellationToken = default)
            {
                return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
            }

            public virtual async Task<bool> Delete(T entity, CancellationToken cancellationToken = default)
            {
                _dbSet.Remove(entity);
                return true;
            }

            public virtual async Task<T?> FindById(int id, CancellationToken cancellationToken = default)
            {
                return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
            }

            public virtual async Task<bool> Update(T entity, CancellationToken cancellationToken = default)
            {
                _dbSet.Update(entity);
                return true;
            }
        }
    }

