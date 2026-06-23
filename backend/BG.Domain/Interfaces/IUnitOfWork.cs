namespace BG.Domain.Interfaces;

public interface IUnitOfWork
{
    IWeaponRepository Weapons { get; }
    IAmmoRepository Crates { get; }
    ILogRepository Logs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}