namespace WhatsAppSalesAgent.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }
    public string? ImageUrl { get; private set; }
    public bool IsActive { get; private set; }

    private Product() { }

    public static Product Create(string name, string description, decimal price, int stockQuantity, string? imageUrl = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");

        if (stockQuantity < 0)
            throw new ArgumentOutOfRangeException(nameof(stockQuantity), "Stock quantity cannot be negative.");

        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Price = price,
            StockQuantity = stockQuantity,
            ImageUrl = imageUrl,
            IsActive = true
        };
    }

    public void Update(string name, string description, decimal price, string? imageUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");

        Name = name;
        Description = description;
        Price = price;
        ImageUrl = imageUrl;
    }

    public void UpdateStock(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Stock quantity cannot be negative.");

        StockQuantity = quantity;
    }

    public void DeductStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity to deduct must be positive.");

        if (StockQuantity < quantity)
            throw new Exceptions.ProductOutOfStockException(Id, Name, quantity, StockQuantity);

        StockQuantity -= quantity;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
