using MediatR;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    string? ImageUrl) : IRequest<Result<Guid>>;
