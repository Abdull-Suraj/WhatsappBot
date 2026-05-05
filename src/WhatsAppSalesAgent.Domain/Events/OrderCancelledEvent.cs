namespace WhatsAppSalesAgent.Domain.Events;

public sealed class OrderCancelledEvent : DomainEventBase
{
    public Guid OrderId { get; }
    public Guid CustomerId { get; }
    public string Reason { get; }

    public OrderCancelledEvent(Guid orderId, Guid customerId, string reason)
    {
        OrderId = orderId;
        CustomerId = customerId;
        Reason = reason;
    }
}
