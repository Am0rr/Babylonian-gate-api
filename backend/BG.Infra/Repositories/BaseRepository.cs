using BG.Domain.Entities;
using BG.Domain.Interfaces;
using BG.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BG.Infra.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(BabylonianDbContext context)
    {
        _dbSet = context.Set<T>();
    }

    public void Add(T item)
    {
        _dbSet.Add(item);
    }

    public void Update(T item)
    {
        _dbSet.Update(item);
    }

    public void Delete(T item)
    {
        _dbSet.Remove(item);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbSet.FindAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public IQueryable<T> Query()
    {
        return _dbSet.AsQueryable();
    }
}