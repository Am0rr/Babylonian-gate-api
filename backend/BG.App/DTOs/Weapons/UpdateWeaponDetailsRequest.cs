namespace BG.App.DTOs.Weapons;

public record UpdateWeaponDetailsRequest(
    Guid Id,
    string Codename,
    string SerialNumber,
    string Caliber
);