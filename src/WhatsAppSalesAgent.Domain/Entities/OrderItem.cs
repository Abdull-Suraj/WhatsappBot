using WhatsAppSalesAgent.Domain.Common;
using WhatsAppSalesAgent.Domain.Exceptions;

namespace WhatsAppSalesAgent.Domain.Entities;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } // Snapshot
    public string ProductImage { get; private set; } // Snapshot
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalPrice { get; private set; }


    //Options(for customizable products)
    public List<ProductOption> SelectedOptions { get; private set; }

    // Navigation Properties
    public Order Order { get; private set; }
    public Product Product { get; private set; }
    private OrderItem() { }

    public OrderItem(
        Guid orderId, 
        Guid productId,
        string productName,
        int quantity, 
        decimal unitPrice,
        List<ProductOption> options = null
        )
    {
        if (productId == Guid.Empty) throw new ArgumentException("ProductId cannot be empty.", nameof(productId));
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be positive.");
        if (unitPrice < 0) throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price cannot be negative.");

        ProductId = productId;
        ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
        
        Quantity = quantity;
        UnitPrice = unitPrice;
        SelectedOptions = options ?? new List<ProductOption>();

        CalculateTotals();
    }

    private void CalculateTotals()
    {
        TotalPrice = UnitPrice * Quantity;

    }

    //private decimal CalculateItemDiscount()
    //{
    //    // Implement item-specific discount logic
    //    return 0;
    //}

    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0) throw new DomainException("Quantity must be positive");
        Quantity = quantity;
        CalculateTotals();
        LastModifiedAt = DateTime.UtcNow;
    }

    public decimal LineTotal => UnitPrice * Quantity;
}
public class ProductOption
{
    public string Name { get; set; }
    public string Value { get; set; }
    public decimal PriceAdjustment { get; set; }
}