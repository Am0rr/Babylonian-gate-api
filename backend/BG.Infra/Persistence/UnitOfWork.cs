using BG.Domain.Interfaces;

namespace BG.Infra.Persistence;

public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private readonly BabylonianDbContext _context;

    public IWeaponRepository Weapons { get; private set; }
    public IAmmoRepository Crates { get; private set; }
    public ILogRepository Logs { get; private set; }
    public ISoldierRepository Soldiers { get; private set; }

    public UnitOfWork(BabylonianDbContext context,
    IWeaponRepository weaponRepository,
    IAmmoRepository ammoRepository,
    ILogRepository logRepository,
    ISoldierRepository soldierRepository)
    {
        _context = context;

        Weapons = weaponRepository;
        Crates = ammoRepository;
        Logs = logRepository;
        Soldiers = soldierRepository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}