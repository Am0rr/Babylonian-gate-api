using BG.Domain.Enums;

namespace BG.Domain.Entities;

public class AmmoCrate : BaseEntity
{
    public string LotNumber { get; private set; } = null!;
    public string Caliber { get; private set; } = null!;
    public AmmoType Type { get; private set; }
    public int Quantity { get; private set; }

    protected AmmoCrate() { }

    private AmmoCrate(string lotNumber, string caliber, int quantity, AmmoType type)
    {
        LotNumber = lotNumber;
        Caliber = caliber;
        Type = type;
        Quantity = quantity;
    }

    public static AmmoCrate Create(string lotNumber, string caliber, int quantity, AmmoType type)
    {
        if (string.IsNullOrWhiteSpace(lotNumber))
            throw new ArgumentException("Lot Number is required.", nameof(lotNumber));

        if (string.IsNullOrWhiteSpace(caliber))
            throw new ArgumentException("Caliber is required.", nameof(caliber));

        if (quantity < 0)
            throw new ArgumentException("Ammo quantity cannot be negative.", nameof(quantity));

        return new AmmoCrate(lotNumber, caliber, quantity, type);
    }

    public void Issue(int amount) => Quantity -= amount;
    public void Restock(int amount) => Quantity += amount;
    public void AdjustQuantity(int actualQuantity) => Quantity = actualQuantity;
    public void CorrectCaliber(string newCaliber) => Caliber = newCaliber;
    public void CorrectType(AmmoType newType) => Type = newType;
    public void CorrectLotNumber(string newLotNumber) => LotNumber = newLotNumber;
}