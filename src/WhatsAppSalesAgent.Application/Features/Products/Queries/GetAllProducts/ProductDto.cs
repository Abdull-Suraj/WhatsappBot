namespace WhatsAppSalesAgent.Application.Features.Products.Queries.GetAllProducts;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    string? ImageUrl,
    bool IsActive);
