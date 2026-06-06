namespace BubbleShop.Domain.Exceptions;

public sealed class ProductOutOfStockException : DomainException
{
    public Guid ProductId { get; }
    public string ProductName { get; }
    public int RequestedQuantity { get; }
    public int AvailableQuantity { get; }

    public ProductOutOfStockException(Guid productId, string productName, int requested, int available)
        : base($"Product '{productName}' (ID: {productId}) is out of stock. Requested: {requested}, Available: {available}.")
    {
        ProductId = productId;
        ProductName = productName;
        RequestedQuantity = requested;
        AvailableQuantity = available;
    }
}
