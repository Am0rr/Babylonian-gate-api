using BG.Domain.Entities;

namespace BG.Domain.Interfaces;

public interface IBaseRepository<T> where T : BaseEntity
{
    void Add(T item);
    void Update(T item);
    void Delete(T item);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T?> GetByIdAysync(Guid id, CancellationToken cancellationToken = default);
}