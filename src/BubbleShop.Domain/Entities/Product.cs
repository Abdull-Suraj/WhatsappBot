using BubbleShop.Domain.Common;
using BubbleShop.Domain.Exceptions;

namespace BubbleShop.Domain.Entities;

public class Product : BaseEntity
{

    public Guid BusinessId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string Barcode { get; private set; }
    public decimal Cost { get; private set; }
    public int StockQuantity { get; private set; }
    public string Category { get; private set; } = string.Empty;
    public string? ImageUrl { get; private set; }
    public bool IsActive { get; private set; }
    public ProductStatus Status { get; private set; }
    public List<string> Images { get; private set; }
    // isRefundable


    // Navigation Properties
    public Business Business { get; private set; }
    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();


    private Product() { }

    public Product(
        Guid businessId,
        string name,
        string description,
        decimal price,
        int stockQuantity,
        string category,
        string? imageUrl)
    {
        BusinessId = businessId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        SetPrice(price);
        UpdateStock(stockQuantity);
        Category = category;
        Images = new List<string>();
        Status = ProductStatus.Active;

    }

    //public static Product Create(string name, string description, decimal price, int stockQuantity, string? imageUrl = null)
    //{
    //    ArgumentException.ThrowIfNullOrWhiteSpace(name);
    //    ArgumentException.ThrowIfNullOrWhiteSpace(description);

    //    if (price < 0)
    //        throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");

    //    if (stockQuantity < 0)
    //        throw new ArgumentOutOfRangeException(nameof(stockQuantity), "Stock quantity cannot be negative.");

    //    return new Product
    //    {
    //        Id = Guid.NewGuid(),
    //        Name = name,
    //        Description = description,
    //        Price = price,
    //        StockQuantity = stockQuantity,
    //        ImageUrl = imageUrl,
    //        IsActive = true
    //    };
    //}

    public void Update(
        string name, 
        string description, 
        decimal price,
        string category,
        string? imageUrl
        )
    {

        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");

        Name = name;
        Description = description;
        Price = price;
        Category = category;
        ImageUrl = imageUrl;
    }

    public void UpdateDetails(string name, string description, string category, string imageUrl)
    {
        Name = name;
        Description = description;
        Category = category;
        ImageUrl = imageUrl;
        LastModifiedAt = DateTime.UtcNow;
    }
    public void SetPrice(decimal price)
    {
        if (price <= 0) throw new DomainException("Price must be greater than zero");
        Price = price;
        LastModifiedAt = DateTime.UtcNow;
    }

    public void UpdateStock(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Stock quantity cannot be negative.");

        StockQuantity = quantity;
        LastModifiedAt = DateTime.UtcNow;
    }

    public void DeductStock(int quantity)       
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity to deduct must be positive.");

        if (StockQuantity < quantity)
            throw new Exceptions.ProductOutOfStockException(Id, Name, quantity, StockQuantity);

        StockQuantity -= quantity;
        LastModifiedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        Status = ProductStatus.Active;
        LastModifiedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = ProductStatus.Inactive;
        LastModifiedAt = DateTime.UtcNow;
    }
}
public enum ProductStatus
{
    Active,
    Inactive,
    OutOfStock,
    Discontinued
}