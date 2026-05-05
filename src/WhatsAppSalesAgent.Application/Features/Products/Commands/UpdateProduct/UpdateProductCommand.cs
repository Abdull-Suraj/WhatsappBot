using MediatR;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid ProductId,
    string Name,
    string Description,
    decimal Price,
    string? ImageUrl,
    bool IsActive) : IRequest<Result>;
