namespace BG.App.DTOs.AmmoCrates;

public record RestockAmmoRequest(
    Guid CrateId,
    int Amount,
    Guid SoldierId
);