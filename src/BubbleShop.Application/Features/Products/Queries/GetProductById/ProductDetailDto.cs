namespace BubbleShop.Application.Features.Products.Queries.GetProductById;

public record ProductDetailDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    string? ImageUrl,
    bool IsActive);
