using MediatR;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid ProductId,
    string Name,
    string Description,
    decimal Price,
    string? ImageUrl,
    bool IsActive) : IRequest<Result>;
