namespace BG.App.DTOs.Soldiers;

public record UpdateSoldierRequest(
    Guid Id,
    string FirstName,
    string LastName,
    string Rank
);