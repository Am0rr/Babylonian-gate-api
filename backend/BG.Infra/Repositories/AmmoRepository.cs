using BG.Domain.Entities.Inventory;
using BG.Domain.Interfaces;
using BG.Infra.Persistence;

namespace BG.Infra.Repositories;

public class AmmoRepository : BaseRepository<AmmoCrate>, IAmmoRepository
{
    public AmmoRepository(BabylonianDbContext context) : base(context) { }
}