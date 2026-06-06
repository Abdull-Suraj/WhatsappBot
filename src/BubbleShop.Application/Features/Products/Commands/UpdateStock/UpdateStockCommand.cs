using MediatR;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.Products.Commands.UpdateStock;

public record UpdateStockCommand(Guid ProductId, int NewQuantity) : IRequest<Result>;
