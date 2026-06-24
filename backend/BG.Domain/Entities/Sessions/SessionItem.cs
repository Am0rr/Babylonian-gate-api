namespace BG.Domain.Entities.Sessions;

public class SessionItem : BaseEntity
{
    public Guid SessionId { get; private set; }
    public Guid WeaponId { get; private set; }
    public Guid? AmmoCrateId { get; private set; }
    public int RoundsFired { get; private set; }
    public Session Session { get; private set; } = null!;

    protected SessionItem() { }

    public SessionItem(Guid sessionId, Guid weaponId, Guid? ammoCrateId, int roundsFired)
    {
        SessionId = sessionId;
        WeaponId = weaponId;
        AmmoCrateId = ammoCrateId;
        RoundsFired = roundsFired;
    }
}