using BG.Domain.Entities.Logging;
using BG.Domain.Interfaces;
using BG.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BG.Infra.Repositories;

public class LogRepository : ILogRepository
{
    private readonly DbSet<OperationLog> _dbSet;

    public LogRepository(BabylonianDbContext context)
    {
        _dbSet = context.Set<OperationLog>();
    }

    public void Add(OperationLog item)
    {
        _dbSet.Add(item);
    }

    public async Task<OperationLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<List<OperationLog>> GetByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
                .AsNoTracking()
                .Where(x => x.RelatedEntityId == entityId)
                .ToListAsync(cancellationToken);
    }

    public async Task<List<OperationLog>> GetRecentAsync(int count, CancellationToken cancellationToken)
    {
        return await _dbSet
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Take(count)
                .ToListAsync(cancellationToken);
    }
}