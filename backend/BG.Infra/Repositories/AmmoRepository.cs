using BG.Domain.Entities;
using BG.Domain.Interfaces;
using BG.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BG.Infra.Repositories;

public class AmmoRepository : BaseRepository<AmmoCrate>, IAmmoRepository
{
    public AmmoRepository(BabylonianDbContext context) : base(context) { }
}