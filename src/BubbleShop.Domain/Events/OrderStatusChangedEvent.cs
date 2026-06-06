namespace BubbleShop.Domain.Events;

public sealed class OrderStatusChangedEvent : DomainEventBase
{
    public Guid OrderId { get; }
    public string PreviousStatus { get; }
    public string NewStatus { get; }

    public OrderStatusChangedEvent(Guid orderId, string previousStatus, string newStatus)
    {
        OrderId = orderId;
        PreviousStatus = previousStatus;
        NewStatus = newStatus;
    }
}
