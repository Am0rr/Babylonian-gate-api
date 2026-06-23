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

    public void ChangeFirstName(string newFirstName) => FirstName = newFirstName;
    public void ChangeLastName(string newLastName) => LastName = newLastName;
    public void ChangeRank(SoldierRank newRank) => Rank = newRank;
}