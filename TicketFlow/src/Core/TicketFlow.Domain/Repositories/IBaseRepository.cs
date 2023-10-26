namespace TicketFlow.Domain.Repositories;

public interface IBaseRepository<T, TFilter>
{
    Task<string> CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<T> GetAsync(TFilter filter, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(TFilter filter, CancellationToken cancellationToken = default);
    IQueryable<T> GetQueryable();
}