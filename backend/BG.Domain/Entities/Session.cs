using BG.Domain.Enums;

namespace BG.Domain.Entities;

public class Session : BaseEntity
{
    public Guid ClientId { get; private set; }
    public Guid InstructorId { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? EndedAt { get; private set; }
    public SessionStatus Status { get; private set; }

    private readonly List<SessionItem> _items = new();
    public IReadOnlyCollection<SessionItem> Items => _items.AsReadOnly();

    protected Session() { }

    public Session(Guid clientId, Guid intructorId)
    {
        ClientId = clientId;
        InstructorId = intructorId;
        StartedAt = DateTime.UtcNow;
        Status = SessionStatus.Active;
    }

    public void AddItem(Guid weaponId, Guid? ammoCrateId, int roundsFired)
    {
        if (Status != SessionStatus.Active)
            throw new InvalidOperationException("Cannot modify a completed session.");

        _items.Add(new SessionItem(Id, weaponId, ammoCrateId, roundsFired));
    }

    public void Complete()
    {
        if (Status != SessionStatus.Active)
            throw new InvalidOperationException("Session is not active.");

        Status = SessionStatus.Completed;
        EndedAt = DateTime.UtcNow;
    }

    public void Cancell()
    {
        if (Status != SessionStatus.Active)
            throw new InvalidOperationException($"Cannot cancel session with status {Status}.");

        Status = SessionStatus.Cancelled;
        EndedAt = DateTime.UtcNow;
    }
}