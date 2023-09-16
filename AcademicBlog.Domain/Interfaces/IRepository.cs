namespace AcademicBlog.Domain.Interfaces;
public interface IRepository<T> where T : class
{
    Task DeleteAsync(int id, bool saveChange = true);
    Task DeleteAsync(T entity, bool saveChange = true);

    Task DeleteRangeAsync(IEnumerable<T> entities, bool saveChange = true);
    T Find(params object[] keyValues);
    Task<T> FindAsync(params object[] keyValues);
    Task<IList<T>> GetAllAsync();
    Task InsertAsync(T entity, bool saveChange = true);

    Task InsertRangeAsync(IEnumerable<T> entities, bool saveChange = true);

    Task UpdateAsync(T entity, bool saveChange = true);

    Task UpdateRangeAsync(IEnumerable<T> entities, bool saveChange = true);


}