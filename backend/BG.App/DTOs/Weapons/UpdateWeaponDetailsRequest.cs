namespace BG.App.DTOs.Weapons;

public record UpdateWeaponDetailsRequest(
    string? Codename,
    string? SerialNumber,
    string? Caliber,
    string? Type
);