namespace BubbleShop.Domain.Events;

public sealed class OrderPlacedEvent : DomainEventBase
{
    public Guid OrderId { get; }
    public Guid CustomerId { get; }

    public OrderPlacedEvent(Guid orderId, Guid customerId)
    {
        OrderId = orderId;
        CustomerId = customerId;
    }
}
