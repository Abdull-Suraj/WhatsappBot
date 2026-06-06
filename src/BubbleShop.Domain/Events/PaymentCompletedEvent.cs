namespace BubbleShop.Domain.Events;

public sealed class PaymentCompletedEvent : DomainEventBase
{
    public Guid OrderId { get; }
    public Guid CustomerId { get; }
    public Guid PaymentId { get; }
    public decimal Amount { get; }

    public PaymentCompletedEvent(Guid orderId, Guid customerId, Guid paymentId, decimal amount)
    {
        OrderId = orderId;
        CustomerId = customerId;
        PaymentId = paymentId;
        Amount = amount;
    }
}
