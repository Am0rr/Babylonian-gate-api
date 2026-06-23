using AutoMapper;
using BG.App.DTOs.AmmoCrates;
using BG.App.DTOs.OperationLogs;
using BG.App.DTOs.Weapons;
using BG.App.DTOs.Soldiers;
using BG.Domain.Entities;

namespace BG.App;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Weapon, WeaponResponse>();
        CreateMap<AmmoCrate, AmmoResponse>();
        CreateMap<Soldier, SoldierResponse>();
        CreateMap<OperationLog, LogResponse>();
    }
}