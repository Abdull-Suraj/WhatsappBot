using WhatsAppSalesAgent.Domain.Enums;

namespace WhatsAppSalesAgent.Domain.Exceptions;

public sealed class InvalidOrderStateException : DomainException
{
    public Guid OrderId { get; }
    public OrderStatus CurrentStatus { get; }
    public string AttemptedOperation { get; }

    public InvalidOrderStateException(Guid orderId, OrderStatus currentStatus, string attemptedOperation)
        : base($"Cannot perform '{attemptedOperation}' on order '{orderId}' in status '{currentStatus}'.")
    {
        OrderId = orderId;
        CurrentStatus = currentStatus;
        AttemptedOperation = attemptedOperation;
    }
}
