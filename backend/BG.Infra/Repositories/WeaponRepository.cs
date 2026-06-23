using BG.Domain.Interfaces;
using BG.Domain.Entities.Inventory;
using BG.Infra.Persistence;

namespace BG.Infra.Repositories;

public class WeaponRepository : BaseRepository<Weapon>, IWeaponRepository
{
    public WeaponRepository(BabylonianDbContext context) : base(context) { }
}