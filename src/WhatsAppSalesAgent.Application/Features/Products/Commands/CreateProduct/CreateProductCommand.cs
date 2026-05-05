using MediatR;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    string? ImageUrl) : IRequest<Result<Guid>>;
