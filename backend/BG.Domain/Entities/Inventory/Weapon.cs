using BG.Domain.Enums;

namespace BG.Domain.Entities.Inventory;

public class Weapon : BaseEntity
{
    public string Codename { get; private set; } = null!;
    public string SerialNumber { get; private set; } = null!;
    public string Caliber { get; private set; } = null!;
    public WeaponType Type { get; private set; }
    public double Condition { get; private set; }
    public WeaponStatus Status { get; private set; }
    public Guid? IssuedToUserId { get; private set; }

    protected Weapon() { }

    public Weapon(string codeName, string serialNumber, string caliber, WeaponType type)
    {
        Codename = codeName;
        SerialNumber = serialNumber;
        Type = type;
        Caliber = caliber;
        Condition = 100.0;
        Status = WeaponStatus.InStorage;
    }

    public void ApplyWear(int roundsFired)
    {
        if (roundsFired < 0) throw new ArgumentException("Rounds cannot be negative.");
        if (roundsFired == 0) return;
        if (Status != WeaponStatus.Deployed)
        {
            throw new InvalidOperationException($"Cannot apply wear. Weapon status is '{Status}', but must be 'Deployed'.");
        }

        var damage = roundsFired * 0.1;
        Condition = Math.Max(0, Condition - damage);
    }

    public void ChangeCodeName(string newCodeName) => Codename = newCodeName;
    public void ChangeSerialNumber(string newSerialNumber) => SerialNumber = newSerialNumber;
    public void ChangeCaliber(string newCaliber) => Caliber = newCaliber;
    public void ChangeType(WeaponType newType) => Type = newType;

    public void IssueTo(Guid userId)
    {
        if (Status != WeaponStatus.InStorage)
            throw new InvalidOperationException("Weapon is not in storage.");

        if (Condition <= 0)
            throw new InvalidOperationException("Cannot issue broken weapon.");

        Status = WeaponStatus.Deployed;
        IssuedToUserId = userId;
    }

    public void ReturnToStorage()
    {
        if (Status == WeaponStatus.InStorage) return;

        if (Status == WeaponStatus.Maintenance)
        {
            Condition = 100.0;
        }

        Status = WeaponStatus.InStorage;
        IssuedToUserId = null;
    }

    public void MarkAsMissing() => Status = WeaponStatus.Missing;

    public void SendToMaintenance()
    {
        if (Status == WeaponStatus.Deployed)
            throw new InvalidOperationException("Cannot send active weapon to maintenance. Return from soldier first.");

        if (Status == WeaponStatus.Missing)
            throw new InvalidOperationException("Cannot repair missing weapon.");

        Status = WeaponStatus.Maintenance;
        IssuedToUserId = null;
    }
}