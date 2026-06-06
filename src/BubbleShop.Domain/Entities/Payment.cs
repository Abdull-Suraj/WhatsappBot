using MediatR;
using BubbleShop.Domain.Common;
using BubbleShop.Domain.Enums;
using BubbleShop.Domain.Exceptions;

namespace BubbleShop.Domain.Entities;

public class Payment : BaseEntity
{

    // Core Identifiers
    public Guid OrderId { get; private set; }
    public Guid BusinessId { get; private set; }
    public Guid? CustomerId { get; private set; }

    // Transaction Identifiers
    public string TransactionReference { get; private set; }
    public string PaymentIntentId { get; private set; } // Stripe/PayPal payment intent ID
    public string ProviderTransactionId { get; private set; } // Gateway-specific transaction ID

    // Payment Details
    public PaymentMethod PaymentMethod { get; private set; }
    public PaymentStatus Status { get; private set; }
    public PaymentType PaymentType { get; private set; }

    // Amounts
    public decimal Amount { get; private set; }
    public decimal AmountPaid { get; private set; }
    public decimal AmountRefunded { get; private set; }
    public decimal Currency { get; private set; }

    // Fees & Commissions
    public decimal PlatformFee { get; private set; } // Your platform's commission
    public decimal PaymentGatewayFee { get; private set; } // Stripe/PayPal fee
    public decimal BusinessEarnings { get; private set; } // Amount business receives

    // Payment Timing
    public DateTime? PaidAt { get; private set; }
    public DateTime? RefundedAt { get; private set; }
    public DateTime? ExpiresAt { get; private set; } // For pending payments

    // Customer Information (snapshot)
    public string CustomerName { get; private set; }
    public string CustomerEmail { get; private set; }
    public string CustomerPhone { get; private set; }
    public string BillingAddress { get; private set; }

    // Payment Gateway Response
    public string GatewayResponse { get; private set; }
    public string GatewayResponseCode { get; private set; }

    // Additional Metadata
    public Dictionary<string, string> Metadata { get; private set; }
    public string FailureReason { get; private set; }
    public int RetryCount { get; private set; }

    // Installment Payment Details
    public int? InstallmentCount { get; private set; }
    public int? CurrentInstallment { get; private set; }
    public decimal? InstallmentAmount { get; private set; }

    // Navigation Properties
    public Order Order { get; private set; }
    public Business Business { get; private set; }
    public Customer Customer { get; private set; }

    // Private collection for payment logs
    private readonly List<PaymentLog> _paymentLogs = new();
    public IReadOnlyCollection<PaymentLog> PaymentLogs => _paymentLogs.AsReadOnly();

    private Payment() { } // EF Core constructor

    // Constructor for new payment
    public Payment(
        Guid orderId,
        Guid businessId,
        decimal amount,
        PaymentMethod paymentMethod,
        Guid? customerId = null,
        string customerName = null,
        string customerEmail = null,
        string customerPhone = null)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        BusinessId = businessId;
        CustomerId = customerId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        Status = PaymentStatus.Pending;
        PaymentType = PaymentType.Full;
        TransactionReference = GenerateTransactionReference();
        CreatedAt = DateTime.UtcNow;

        // Set expiry to 24 hours from now
        ExpiresAt = DateTime.UtcNow.AddHours(24);

        // Store customer snapshot
        CustomerName = customerName;
        CustomerEmail = customerEmail;
        CustomerPhone = customerPhone;

        // Initialize metadata
        Metadata = new Dictionary<string, string>();

        // Calculate fees
        CalculateFees();

        AddPaymentLog(PaymentStatus.Pending, "Payment initiated");
        AddDomainEvent(new PaymentInitiatedEvent(Id, OrderId, TransactionReference, Amount));
    }

    // Calculate platform and gateway fees
    private void CalculateFees()
    {
        // Platform fee: 10% of amount
        PlatformFee = Amount * 0.10m;

        // Payment gateway fee: 2.9% + $0.30 (Stripe standard)
        PaymentGatewayFee = (Amount * 0.029m) + 0.30m;

        // Business earnings = Amount - PlatformFee - PaymentGatewayFee
        BusinessEarnings = Amount - PlatformFee - PaymentGatewayFee;

        if (BusinessEarnings < 0) BusinessEarnings = 0;
    }

    // Mark payment as successful
    public void MarkAsSuccessful(string gatewayResponse, string providerTransactionId = null)
    {
        if (Status != PaymentStatus.Pending && Status != PaymentStatus.Processing)
            throw new DomainException($"Cannot mark payment as successful from {Status} status");

        Status = PaymentStatus.Successful;
        AmountPaid = Amount;
        PaidAt = DateTime.UtcNow;
        GatewayResponse = gatewayResponse;
        ProviderTransactionId = providerTransactionId;
        LastModifiedAt = DateTime.UtcNow;

        AddPaymentLog(PaymentStatus.Successful, "Payment completed successfully");
        AddDomainEvent(new PaymentSuccessfulEvent(Id, OrderId, TransactionReference, Amount));
    }

    // Mark payment as processing (for async payments)
    public void MarkAsProcessing(string paymentIntentId = null)
    {
        if (Status != PaymentStatus.Pending)
            throw new DomainException($"Cannot mark payment as processing from {Status} status");

        Status = PaymentStatus.Processing;
        PaymentIntentId = paymentIntentId;
        LastModifiedAt = DateTime.UtcNow;

        AddPaymentLog(PaymentStatus.Processing, "Payment is being processed");
    }

    // Mark payment as failed
    public void MarkAsFailed(string failureReason, string gatewayResponse = null)
    {
        if (Status != PaymentStatus.Pending && Status != PaymentStatus.Processing)
            throw new DomainException($"Cannot mark payment as failed from {Status} status");

        Status = PaymentStatus.Failed;
        FailureReason = failureReason;
        GatewayResponse = gatewayResponse;
        LastModifiedAt = DateTime.UtcNow;

        AddPaymentLog(PaymentStatus.Failed, $"Payment failed: {failureReason}");
        AddDomainEvent(new PaymentFailedEvent(Id, OrderId, TransactionReference, failureReason));
    }

    // Process refund (full or partial)
    public void Refund(decimal refundAmount, string reason = null)
    {
        if (Status != PaymentStatus.Successful)
            throw new DomainException("Only successful payments can be refunded");

        if (refundAmount <= 0)
            throw new DomainException("Refund amount must be positive");

        if (AmountRefunded + refundAmount > AmountPaid)
            throw new DomainException("Refund amount exceeds paid amount");

        AmountRefunded += refundAmount;

        if (AmountRefunded >= AmountPaid)
        {
            Status = PaymentStatus.Refunded;
            RefundedAt = DateTime.UtcNow;
        }
        else
        {
            Status = PaymentStatus.PartiallyRefunded;
        }

        LastModifiedAt = DateTime.UtcNow;

        AddPaymentLog(PaymentStatus.Refunded, $"Refunded {refundAmount:C}. Reason: {reason ?? "No reason provided"}");
        AddDomainEvent(new PaymentRefundedEvent(Id, OrderId, TransactionReference, refundAmount, reason));
    }

    // Void/cancel payment before it's captured
    public void Void(string reason = null)
    {
        if (Status != PaymentStatus.Pending && Status != PaymentStatus.Processing)
            throw new DomainException("Only pending or processing payments can be voided");

        Status = PaymentStatus.Voided;
        LastModifiedAt = DateTime.UtcNow;

        AddPaymentLog(PaymentStatus.Voided, $"Payment voided. Reason: {reason ?? "No reason provided"}");
        AddDomainEvent(new PaymentVoidedEvent(Id, OrderId, TransactionReference, reason));
    }

    // Process partial payment (for installment plans)
    public void ProcessPartialPayment(decimal partialAmount, string gatewayResponse = null)
    {
        if (PaymentType != PaymentType.Installment)
            throw new DomainException("Partial payments only allowed for installment payments");

        if (partialAmount <= 0)
            throw new DomainException("Partial payment amount must be positive");

        if (AmountPaid + partialAmount > Amount)
            throw new DomainException("Partial payment exceeds total amount");

        AmountPaid += partialAmount;
        CurrentInstallment = (CurrentInstallment ?? 0) + 1;
        GatewayResponse = gatewayResponse;
        LastModifiedAt = DateTime.UtcNow;

        if (AmountPaid >= Amount)
        {
            Status = PaymentStatus.Successful;
            PaidAt = DateTime.UtcNow;
            AddPaymentLog(PaymentStatus.Successful, $"Final installment paid. Total paid: {AmountPaid:C}");
        }
        else
        {
            Status = PaymentStatus.PartiallyPaid;
            AddPaymentLog(PaymentStatus.PartiallyPaid, $"Partial payment of {partialAmount:C} received. Remaining: {Amount - AmountPaid:C}");
        }

        AddDomainEvent(new PartialPaymentReceivedEvent(Id, OrderId, TransactionReference, partialAmount, AmountPaid));
    }

    // Setup installment plan
    public void SetupInstallmentPlan(int installmentCount, decimal? installmentAmount = null)
    {
        if (Status != PaymentStatus.Pending)
            throw new DomainException("Installment plan can only be set up for pending payments");

        if (installmentCount <= 0 || installmentCount > 12)
            throw new DomainException("Installment count must be between 1 and 12");

        InstallmentCount = installmentCount;
        PaymentType = PaymentType.Installment;
        InstallmentAmount = installmentAmount ?? (Amount / installmentCount);
        CurrentInstallment = 0;
        LastModifiedAt = DateTime.UtcNow;

        AddPaymentLog(PaymentStatus.Pending, $"Installment plan set up: {installmentCount} payments of {InstallmentAmount:C}");
    }

    // Check if payment is expired
    public bool IsExpired()
    {
        return ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value && Status == PaymentStatus.Pending;
    }

    // Extend payment expiry
    public void ExtendExpiry(int hours)
    {
        if (Status != PaymentStatus.Pending)
            throw new DomainException("Only pending payments can be extended");

        ExpiresAt = DateTime.UtcNow.AddHours(hours);
        LastModifiedAt = DateTime.UtcNow;

        AddPaymentLog(PaymentStatus.Pending, $"Payment expiry extended by {hours} hours");
    }

    // Retry failed payment
    public void Retry()
    {
        if (Status != PaymentStatus.Failed)
            throw new DomainException("Only failed payments can be retried");

        if (RetryCount >= 3)
            throw new DomainException("Maximum retry attempts (3) reached");

        Status = PaymentStatus.Pending;
        FailureReason = null;
        RetryCount++;
        LastModifiedAt = DateTime.UtcNow;

        AddPaymentLog(PaymentStatus.Pending, $"Retry attempt #{RetryCount}");
    }

    // Add custom metadata
    public void AddMetadata(string key, string value)
    {
        Metadata[key] = value;
        LastModifiedAt = DateTime.UtcNow;
    }

    // Get remaining balance
    public decimal GetRemainingBalance()
    {
        return Amount - AmountPaid;
    }

    // Check if fully paid
    public bool IsFullyPaid => AmountPaid >= Amount;

    // Get refundable amount
    public decimal GetRefundableAmount()
    {
        return AmountPaid - AmountRefunded;
    }

    // Get payment status description
    public string GetStatusDescription()
    {
        return Status switch
        {
            PaymentStatus.Pending => "Awaiting payment",
            PaymentStatus.Processing => "Processing payment",
            PaymentStatus.Successful => "Payment completed",
            PaymentStatus.Failed => "Payment failed",
            PaymentStatus.Refunded => "Fully refunded",
            PaymentStatus.PartiallyRefunded => $"Partially refunded ({AmountRefunded:C} of {AmountPaid:C})",
            PaymentStatus.PartiallyPaid => $"Partially paid ({AmountPaid:C} of {Amount:C})",
            PaymentStatus.Voided => "Payment voided",
            _ => "Unknown status"
        };
    }

    // Add payment log entry
    private void AddPaymentLog(PaymentStatus status, string message)
    {
        _paymentLogs.Add(new PaymentLog
        {
            Status = status,
            Message = message,
            Timestamp = DateTime.UtcNow,
            Amount = AmountPaid
        });
    }

    // Generate unique transaction reference
    private static string GenerateTransactionReference()
    {
        return $"TXN-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid():N}"[..20].ToUpper();
    }

    // Update billing address
    public void UpdateBillingAddress(string address)
    {
        if (Status != PaymentStatus.Pending)
            throw new DomainException("Billing address can only be updated for pending payments");

        BillingAddress = address;
        LastModifiedAt = DateTime.UtcNow;
    }

    // Get payment summary
    public PaymentSummary GetSummary()
    {
        return new PaymentSummary
        {
            TransactionReference = TransactionReference,
            Amount = Amount,
            AmountPaid = AmountPaid,
            AmountRefunded = AmountRefunded,
            Status = Status,
            PaymentMethod = PaymentMethod,
            PlatformFee = PlatformFee,
            PaymentGatewayFee = PaymentGatewayFee,
            BusinessEarnings = BusinessEarnings,
            PaidAt = PaidAt,
            RefundedAt = RefundedAt
        };
    }
}

// Enums
public enum PaymentStatus
{
    Pending,           // Initial state, awaiting payment
    Processing,        // Payment being processed (async)
    Successful,        // Payment completed successfully
    Failed,           // Payment failed
    Refunded,         // Fully refunded
    PartiallyRefunded, // Partially refunded
    PartiallyPaid,    // Partial payment received (installments)
    Voided            // Payment voided/cancelled before capture
}

public enum PaymentMethod
{
    CreditCard,
    DebitCard,
    BankTransfer,
    MobileMoney,      // M-Pesa, PayPal, etc.
    Wallet,           // Platform wallet
    CashOnDelivery,
    Cryptocurrency
}

public enum PaymentType
{
    Full,             // One-time full payment
    Installment,      // Multiple payments
    Subscription      // Recurring payments
}

// Payment Log Entity
public class PaymentLog
{
    public Guid Id { get; set; }
    public Guid PaymentId { get; set; }
    public PaymentStatus Status { get; set; }
    public string Message { get; set; }
    public decimal? Amount { get; set; }
    public DateTime Timestamp { get; set; }

    public Payment Payment { get; set; }
}

// Payment Summary DTO
public class PaymentSummary
{
    public string TransactionReference { get; set; }
    public decimal Amount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal AmountRefunded { get; set; }
    public PaymentStatus Status { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal PlatformFee { get; set; }
    public decimal PaymentGatewayFee { get; set; }
    public decimal BusinessEarnings { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime? RefundedAt { get; set; }
    public string StatusDescription => GetStatusDescription();

    private string GetStatusDescription()
    {
        return Status switch
        {
            PaymentStatus.Pending => "Awaiting payment",
            PaymentStatus.Processing => "Processing payment",
            PaymentStatus.Successful => "Payment completed",
            PaymentStatus.Failed => "Payment failed",
            PaymentStatus.Refunded => "Fully refunded",
            PaymentStatus.PartiallyRefunded => $"Partially refunded ({AmountRefunded:C} of {AmountPaid:C})",
            PaymentStatus.PartiallyPaid => $"Partially paid ({AmountPaid:C} of {Amount:C})",
            PaymentStatus.Voided => "Payment voided",
            _ => "Unknown status"
        };
    }
}

// Domain Events
public record PaymentInitiatedEvent : INotification
{
    public Guid PaymentId { get; }
    public Guid OrderId { get; }
    public string TransactionReference { get; }
    public decimal Amount { get; }
    public DateTime OccurredOn { get; }

    public PaymentInitiatedEvent(Guid paymentId, Guid orderId, string transactionReference, decimal amount)
    {
        PaymentId = paymentId;
        OrderId = orderId;
        TransactionReference = transactionReference;
        Amount = amount;
        OccurredOn = DateTime.UtcNow;
    }
}

public record PaymentSuccessfulEvent : INotification
{
    public Guid PaymentId { get; }
    public Guid OrderId { get; }
    public string TransactionReference { get; }
    public decimal Amount { get; }
    public DateTime OccurredOn { get; }

    public PaymentSuccessfulEvent(Guid paymentId, Guid orderId, string transactionReference, decimal amount)
    {
        PaymentId = paymentId;
        OrderId = orderId;
        TransactionReference = transactionReference;
        Amount = amount;
        OccurredOn = DateTime.UtcNow;
    }
}

public record PaymentFailedEvent : INotification
{
    public Guid PaymentId { get; }
    public Guid OrderId { get; }
    public string TransactionReference { get; }
    public string FailureReason { get; }
    public DateTime OccurredOn { get; }

    public PaymentFailedEvent(Guid paymentId, Guid orderId, string transactionReference, string failureReason)
    {
        PaymentId = paymentId;
        OrderId = orderId;
        TransactionReference = transactionReference;
        FailureReason = failureReason;
        OccurredOn = DateTime.UtcNow;
    }
}

public record PaymentRefundedEvent : INotification
{
    public Guid PaymentId { get; }
    public Guid OrderId { get; }
    public string TransactionReference { get; }
    public decimal RefundAmount { get; }
    public string Reason { get; }
    public DateTime OccurredOn { get; }

    public PaymentRefundedEvent(Guid paymentId, Guid orderId, string transactionReference, decimal refundAmount, string reason)
    {
        PaymentId = paymentId;
        OrderId = orderId;
        TransactionReference = transactionReference;
        RefundAmount = refundAmount;
        Reason = reason;
        OccurredOn = DateTime.UtcNow;
    }
}

public record PaymentVoidedEvent : INotification
{
    public Guid PaymentId { get; }
    public Guid OrderId { get; }
    public string TransactionReference { get; }
    public string Reason { get; }
    public DateTime OccurredOn { get; }

    public PaymentVoidedEvent(Guid paymentId, Guid orderId, string transactionReference, string reason)
    {
        PaymentId = paymentId;
        OrderId = orderId;
        TransactionReference = transactionReference;
        Reason = reason;
        OccurredOn = DateTime.UtcNow;
    }
}

public record PartialPaymentReceivedEvent : INotification
{
    public Guid PaymentId { get; }
    public Guid OrderId { get; }
    public string TransactionReference { get; }
    public decimal AmountReceived { get; }
    public decimal TotalPaid { get; }
    public DateTime OccurredOn { get; }

    public PartialPaymentReceivedEvent(Guid paymentId, Guid orderId, string transactionReference, decimal amountReceived, decimal totalPaid)
    {
        PaymentId = paymentId;
        OrderId = orderId;
        TransactionReference = transactionReference;
        AmountReceived = amountReceived;
        TotalPaid = totalPaid;
        OccurredOn = DateTime.UtcNow;
    }
}