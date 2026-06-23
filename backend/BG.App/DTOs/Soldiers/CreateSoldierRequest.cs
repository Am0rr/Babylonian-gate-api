namespace BG.App.DTOs.Soldiers;

public record CreateSoldierRequest(
    string FirstName,
    string LastName,
    string Rank
);