namespace BG.App.DTOs.Weapons;

public record ReturnWeaponToStorageRequest(
    Guid WeaponId,
    int RoundsFired
);