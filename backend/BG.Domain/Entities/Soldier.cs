using BG.Domain.Enums;

namespace BG.Domain.Entities;

public class Soldier : BaseEntity
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public SoldierRank Rank { get; private set; }

    protected Soldier() { }

    private Soldier(string firstName, string lastName, SoldierRank rank)
    {
        FirstName = firstName;
        LastName = lastName;
        Rank = rank;
    }

    public static Soldier Create(string firstName, string lastName, SoldierRank rank)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First Name is required", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last Name is required", nameof(lastName));


        return new Soldier(firstName, lastName, rank);
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