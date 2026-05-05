using MediatR;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.Products.Commands.UpdateStock;

public record UpdateStockCommand(Guid ProductId, int NewQuantity) : IRequest<Result>;
