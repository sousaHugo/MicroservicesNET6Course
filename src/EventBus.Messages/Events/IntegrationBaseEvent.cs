namespace EventBus.Messages.Events;

public class IntegrationBaseEvent
{
    public IntegrationBaseEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }
    public IntegrationBaseEvent(Guid Id, DateTime CreationDate)
    {
        this.Id = Id;
        this.CreationDate = CreationDate;
    }
    public Guid Id { get; private set; }
    public DateTime CreationDate { get; private set; }
}
