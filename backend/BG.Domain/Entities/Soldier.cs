using BG.Domain.Enums;

namespace BG.Domain.Entities;

public class Soldier : BaseEntity
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public SoldierRank Rank { get; private set; }

    protected Soldier() { }

    public Soldier(string firstName, string lastName, SoldierRank rank)
    {
        FirstName = firstName;
        LastName = lastName;
        Rank = rank;
    }

    public void UpdateName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Fist/Last Name cannot be empty.");
        }

        FirstName = firstName;
        LastName = lastName;
    }

    public void UpdateRank(SoldierRank newRank) => Rank = newRank;
}