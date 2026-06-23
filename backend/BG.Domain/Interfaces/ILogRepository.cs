using BG.Domain.Entities.Logging;

namespace BG.Domain.Interfaces;

public interface ILogRepository
{
    void Add(OperationLog item);
    Task<OperationLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<OperationLog>> GetByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default);
    Task<List<OperationLog>> GetRecentAsync(int count, CancellationToken cancellationToken = default);
}