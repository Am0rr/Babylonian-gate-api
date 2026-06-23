namespace BG.App.DTOs.AmmoCrates;

public record AuditAmmoInventoryRequest(
    Guid CrateId,
    int ActualQuantity
);