using System.Linq.Expressions;

namespace AcademicBlog.Domain.Interfaces;
public interface IRepository<T> where T : class
{
    Task<int> CreateAsync(T entity);
    Task<T> DeleteAsync(T entity);
    Task<T> DeleteAsync(int id);
    Task<T> UpdateAsync(T entity);
    Task<T> UpdateAsync(int id, T entity);

    Task<T> GetByIdAsync(int id);
    Task<T> GetByConditionAsync(Expression<Func<T, bool>> expression);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetMultiByConditionAsync(Expression<Func<T, bool>> expression);
    Task<IEnumerable<T>> GetMultiPagingAsync(Expression<Func<T, bool>> expression, int index = 0, int size = 10);
    Task<int> CountAsync(Expression<Func<T, bool>> expression);
    }