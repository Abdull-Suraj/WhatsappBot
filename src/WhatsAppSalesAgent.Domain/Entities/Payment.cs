using WhatsAppSalesAgent.Domain.Enums;

namespace WhatsAppSalesAgent.Domain.Entities;

public class Payment
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public string Provider { get; private set; } = string.Empty;
    public PaymentStatus Status { get; private set; }
    public string? TransactionId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime? PaidAt { get; private set; }

    private Payment() { }

    public static Payment Create(Guid orderId, string provider, decimal amount)
    {
        if (orderId == Guid.Empty) throw new ArgumentException("OrderId cannot be empty.", nameof(orderId));
        ArgumentException.ThrowIfNullOrWhiteSpace(provider);
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");

        return new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            Provider = provider,
            Status = PaymentStatus.Pending,
            Amount = amount
        };
    }

    public void Complete(string transactionId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(transactionId);
        TransactionId = transactionId;
        Status = PaymentStatus.Completed;
        PaidAt = DateTime.UtcNow;
    }

    public void Fail() => Status = PaymentStatus.Failed;

    public void Refund() => Status = PaymentStatus.Refunded;
}
