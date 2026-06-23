namespace BG.App.DTOs.Weapons;

public record CreateWeaponRequest(
    string CodeName,
    string SerialNumber,
    string Caliber,
    string Type
);