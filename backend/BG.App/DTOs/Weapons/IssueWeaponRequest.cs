namespace BG.App.DTOs.Weapons;

public record IssueWeaponRequest(
    Guid WeaponId,
    Guid SoldierId
);