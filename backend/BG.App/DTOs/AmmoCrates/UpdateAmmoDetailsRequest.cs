namespace BG.App.DTOs.AmmoCrates;

public record UpdateAmmoDetailsRequest(
    string? LotNumber,
    string? Caliber,
    string? Type
);