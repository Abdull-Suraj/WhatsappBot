namespace WhatsAppSalesAgent.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    private OrderItem() { }

    public static OrderItem Create(Guid orderId, Guid productId, int quantity, decimal unitPrice)
    {
        if (orderId == Guid.Empty) throw new ArgumentException("OrderId cannot be empty.", nameof(orderId));
        if (productId == Guid.Empty) throw new ArgumentException("ProductId cannot be empty.", nameof(productId));
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be positive.");
        if (unitPrice < 0) throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price cannot be negative.");

        return new OrderItem
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = unitPrice
        };
    }

    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be positive.");

        Quantity = quantity;
    }

    public decimal LineTotal => UnitPrice * Quantity;
}
