namespace Reminders.DAL.Interfaces;

public interface IBaseRepository<T, TFilter>
{
    Task<int> CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<T> GetAsync(TFilter filter);
    Task<IEnumerable<T>> GetAllAsync();
    IQueryable<T> GetQueryable();
}