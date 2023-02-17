
using System.Linq.Expressions;

namespace Core.Data
{
    public interface IRepository<T,IdType>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<ICollection<T>> GetAllAsync(Expression<Func<T,bool>> Filter);
        Task<T> GetAsync(IdType Id);
        Task<T> GetAsync(Expression<Func<T, bool>> Filter);
        Task<bool> CreateAsync(T item);
        Task<bool> UpdateAsync(T item);
        Task<bool> RemoveAsync(IdType Id);
    }
}