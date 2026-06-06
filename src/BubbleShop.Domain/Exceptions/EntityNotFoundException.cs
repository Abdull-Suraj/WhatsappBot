namespace BubbleShop.Domain.Exceptions;

public sealed class EntityNotFoundException : DomainException
{
    public string EntityName { get; }
    public object EntityId { get; }

    public EntityNotFoundException(string entityName, object entityId)
        : base($"Entity '{entityName}' with identifier '{entityId}' was not found.")
    {
        EntityName = entityName;
        EntityId = entityId;
    }
}
