namespace BG.App.DTOs.Soldiers;

public record SoldierResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Rank
);