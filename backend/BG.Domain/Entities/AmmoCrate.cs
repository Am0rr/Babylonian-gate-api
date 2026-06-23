using BG.Domain.Enums;

namespace BG.Domain.Entities;

public class AmmoCrate : BaseEntity
{
    public string LotNumber { get; private set; } = null!;
    public string Caliber { get; private set; } = null!;
    public AmmoType Type { get; private set; }
    public int Quantity { get; private set; }

    protected AmmoCrate() { }

    public AmmoCrate(string lotNumber, string caliber, int quantity, AmmoType type = AmmoType.None)
    {
        LotNumber = lotNumber;
        Caliber = caliber;
        Quantity = quantity;
    }

    public void Issue(int amount) => Quantity -= amount;
    public void Restock(int amount) => Quantity += amount;
    public void AdjustQuantity(int actualQuantity) => Quantity = actualQuantity;
    public void ChangeCaliber(string newCaliber) => Caliber = newCaliber;
    public void ChangeType(AmmoType newType) => Type = newType;
    public void ChangeLotNumber(string newLotNumber) => LotNumber = newLotNumber;
}