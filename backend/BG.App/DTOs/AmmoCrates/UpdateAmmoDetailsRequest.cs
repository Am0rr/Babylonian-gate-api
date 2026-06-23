namespace BG.App.DTOs.AmmoCrates;

public record UpdateAmmoDetailsRequest(
    Guid Id,
    string LotNumber,
    string Caliber,
    string Type
);