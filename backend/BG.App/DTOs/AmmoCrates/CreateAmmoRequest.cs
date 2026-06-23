namespace BG.App.DTOs.AmmoCrates;

public record CreateAmmoRequest(
    string LotNumber,
    string Caliber,
    int Quantity,
    string Type
);