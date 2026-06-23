using BG.Domain.Entities;
using BG.Domain.Interfaces;
using BG.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BG.Infra.Repositories;

public class SoldierRepository : BaseRepository<Soldier>, ISoldierRepository
{
    public SoldierRepository(BabylonianDbContext context) : base(context) { }
}