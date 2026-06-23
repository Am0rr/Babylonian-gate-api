namespace BG.App.DTOs.Weapons;

public record WeaponResponse(
    Guid Id,
    string Codename,
    string SerialNumber,
    string Caliber,
    string Type,
    string Status,
    double? Condition,
    Guid? IssuedToSoldierId
);