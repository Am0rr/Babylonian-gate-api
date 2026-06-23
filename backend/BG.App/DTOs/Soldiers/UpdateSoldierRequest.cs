namespace BG.App.DTOs.Soldiers;

public record UpdateSoldierRequest(
    string? FirstName,
    string? LastName,
    string? Rank
);