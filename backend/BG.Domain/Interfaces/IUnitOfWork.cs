namespace BG.Domain.Interfaces;

public interface IUnitOfWork
{
    IWeaponRepository Weapons { get; }
    IAmmoRepository Crates { get; }
    ILogRepository Logs { get; }
    ISoldierRepository Soldiers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}