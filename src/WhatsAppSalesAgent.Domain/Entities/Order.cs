using WhatsAppSalesAgent.Domain.Common;
using WhatsAppSalesAgent.Domain.Enums;
using WhatsAppSalesAgent.Domain.Events;
using WhatsAppSalesAgent.Domain.Exceptions;

namespace WhatsAppSalesAgent.Domain.Entities;

public class Order : BaseEntity
{

    public string OrderNumber { get; private set; }
    public Guid BusinessId { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    //public int Quantity { get; private set; }

 
    public OrderType OrderType { get; private set; }


    // Amounts
    public decimal Subtotal { get; private set; }
    public decimal TotalAmount { get; private set; }


    // Customer Information (snapshot at order time)
    public string CustomerName { get; private set; }
    public string CustomerEmail { get; private set; }
    public string CustomerPhone { get; private set; }
    public string ShippingAddress { get; private set; }
    public string BillingAddress { get; private set; }
    public string SpecialInstructions { get; private set; }

    // Dates
    public DateTime? PaidAt { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? ProcessedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    // Cancellation
    public string CancellationReason { get; private set; }

    // Navigation Properties
    public Business Business { get; private set; }
    public Customer Customer { get; private set; }
    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public Payment Payment { get; private set; }
    public Delivery Delivery { get; private set; }


    private Order() { }

    public Order(Guid businessId,
        Guid customerId,
        string customerName,
        string customerPhone,
        string shippingAddress,
        List<OrderItem> items)
    {

        OrderNumber = GenerateOrderNumber();
        BusinessId = businessId;
        CustomerId = customerId;
        CustomerName = customerName;
        CustomerPhone = customerPhone;
        ShippingAddress = shippingAddress;
        Status = OrderStatus.Pending;
        OrderType = OrderType.Standard;

        foreach (var item in items)
        {
            _orderItems.Add(item);
            Subtotal += item.TotalPrice;
        }

        CalculateTotals();
    }

    public void AddItem(OrderItem item)
    {
        _orderItems.Add(item);
        Subtotal += item.TotalPrice;
        CalculateTotals();
        LastModifiedAt = DateTime.UtcNow;
    }

    public void RemoveItem(Guid productId)
    {
        var item = _orderItems.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            _orderItems.Remove(item);
            Subtotal -= item.TotalPrice;
            CalculateTotals();
            LastModifiedAt = DateTime.UtcNow;
        }
    }

    private void CalculateTotals()
    {

        TotalAmount = Subtotal;
    }

    public void ConfirmPayment(Payment payment)
    {
        if (Status != OrderStatus.Pending && Status != OrderStatus.PaymentPending)
            throw new DomainException("Order cannot be confirmed for payment");

        Payment = payment;
        Status = OrderStatus.PaymentReceived;
        PaidAt = DateTime.UtcNow;
        LastModifiedAt = DateTime.UtcNow;

        AddDomainEvent(new OrderPaidEvent(Id, OrderNumber, TotalAmount));
    }

    public void ConfirmOrder()
    {
        if (Status != OrderStatus.PaymentReceived)
            throw new DomainException("Order must be paid before confirmation");

        Status = OrderStatus.Confirmed;
        ConfirmedAt = DateTime.UtcNow;
        LastModifiedAt = DateTime.UtcNow;

        AddDomainEvent(new OrderConfirmedEvent(Id, OrderNumber));
    }
    public void StartProcessing()
    {
        if (Status != OrderStatus.Confirmed)
            throw new DomainException("Order must be confirmed before processing");

        Status = OrderStatus.Processing;
        ProcessedAt = DateTime.UtcNow;
        LastModifiedAt = DateTime.UtcNow;

        AddDomainEvent(new OrderProcessingStartedEvent(Id, OrderNumber));
    }

    public void CompleteOrder()
    {
        if (Status != OrderStatus.Shipped && Status != OrderStatus.Delivered)
            throw new DomainException("Order must be shipped before completion");

        Status = OrderStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        LastModifiedAt = DateTime.UtcNow;

        AddDomainEvent(new OrderCompletedEvent(Id, OrderNumber));
    }

    public void CancelOrder(string reason)
    {
        if (Status == OrderStatus.Completed || Status == OrderStatus.Delivered)
            throw new DomainException("Cannot cancel completed or delivered order");

        Status = OrderStatus.Cancelled;
        CancellationReason = reason;
        CancelledAt = DateTime.UtcNow;
        LastModifiedAt = DateTime.UtcNow;


        AddDomainEvent(new OrderCancelledEvent(Id, OrderNumber, reason));
    }

    public void RequestPayment()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Order must be pending for payment request");

        Status = OrderStatus.PaymentPending;
        LastModifiedAt = DateTime.UtcNow;

        AddDomainEvent(new PaymentRequestedEvent(Id, OrderNumber, TotalAmount));
    }

    private static string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid():N}"[..8].ToUpper();
    }

    public void UpdateShippingAddress(string address)
    {
        if (Status != OrderStatus.Pending && Status != OrderStatus.PaymentPending)
            throw new DomainException("Shipping address can only be updated before payment");

        ShippingAddress = address;
        LastModifiedAt = DateTime.UtcNow;
    }

    public decimal GetRemainingBalance()
    {
        if (Payment?.AmountPaid >= TotalAmount) return 0;
        return TotalAmount - (Payment?.AmountPaid ?? 0);
    }
    // method for updating wallet balance if successful 
}
public enum OrderStatus
{
    Pending,           // Initial state
    PaymentPending,    // Awaiting payment
    PaymentReceived,   // Payment confirmed
    Confirmed,         // Order confirmed by business
    Processing,        // Being prepared
    Shipped,           // Out for delivery
    Delivered,         // Customer received
    Completed,         // Order completed
    Cancelled,         // Cancelled
    Refunded           // Refunded
}

public enum OrderType
{
    Standard,
    Express,
    Scheduled,
    Pickup
}