namespace BG.App.DTOs.AmmoCrates;

public record AmmoResponse(
    Guid Id,
    string LotNumber,
    string Caliber,
    int Quantity,
    string Type
);