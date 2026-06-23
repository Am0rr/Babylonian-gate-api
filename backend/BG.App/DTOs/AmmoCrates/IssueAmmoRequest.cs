namespace BG.App.DTOs.AmmoCrates;

public record IssueAmmoRequest(
    Guid CrateId,
    int Amount,
    Guid SoldierId
);