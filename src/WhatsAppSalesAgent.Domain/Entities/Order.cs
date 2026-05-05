using WhatsAppSalesAgent.Domain.Enums;
using WhatsAppSalesAgent.Domain.Events;
using WhatsAppSalesAgent.Domain.Exceptions;

namespace WhatsAppSalesAgent.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public ICollection<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();
    public Payment? Payment { get; private set; }
    public Delivery? Delivery { get; private set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Order() { }

    public static Order Create(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("CustomerId cannot be empty.", nameof(customerId));

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            Status = OrderStatus.Pending,
            TotalAmount = 0m,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        order._domainEvents.Add(new OrderPlacedEvent(order.Id, customerId));
        return order;
    }

    public void AddItem(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be positive.");

        if (Status != OrderStatus.Pending)
            throw new InvalidOrderStateException(Id, Status, "AddItem");

        var existingItem = OrderItems.FirstOrDefault(i => i.ProductId == product.Id);
        if (existingItem is not null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            var item = OrderItem.Create(Id, product.Id, quantity, product.Price);
            ((List<OrderItem>)OrderItems).Add(item);
        }

        RecalculateTotal();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOrderStateException(Id, Status, "Confirm");

        if (!OrderItems.Any())
            throw new DomainException("Cannot confirm an order with no items.");

        Status = OrderStatus.Confirmed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsPaid(Payment payment)
    {
        ArgumentNullException.ThrowIfNull(payment);

        if (Status != OrderStatus.Confirmed)
            throw new InvalidOrderStateException(Id, Status, "MarkAsPaid");

        Payment = payment;
        Status = OrderStatus.Paid;
        UpdatedAt = DateTime.UtcNow;
        _domainEvents.Add(new PaymentCompletedEvent(Id, CustomerId, payment.Id, TotalAmount));
    }

    public void Dispatch(Delivery delivery)
    {
        ArgumentNullException.ThrowIfNull(delivery);

        if (Status != OrderStatus.Paid)
            throw new InvalidOrderStateException(Id, Status, "Dispatch");

        Delivery = delivery;
        Status = OrderStatus.Dispatched;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsDelivered()
    {
        if (Status != OrderStatus.Dispatched)
            throw new InvalidOrderStateException(Id, Status, "MarkAsDelivered");

        Status = OrderStatus.Delivered;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel(string reason)
    {
        if (Status is OrderStatus.Delivered or OrderStatus.Dispatched)
            throw new InvalidOrderStateException(Id, Status, "Cancel");

        Status = OrderStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
        _domainEvents.Add(new OrderCancelledEvent(Id, CustomerId, reason));
    }

    public void ClearDomainEvents() => _domainEvents.Clear();

    private void RecalculateTotal()
    {
        TotalAmount = OrderItems.Sum(i => i.UnitPrice * i.Quantity);
    }
}
