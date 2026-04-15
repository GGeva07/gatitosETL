using gatitosEtl.Models.DIMS;
using System.Linq.Expressions;

namespace gatitosEtl.Data.interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<int> InsertIfNotExistsAsync(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    }
}