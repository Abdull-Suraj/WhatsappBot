using MediatR;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.Orders.Commands.CreateOrder;

public record CreateOrderCommand(
    Guid CustomerId,
    List<OrderLineItem> Items) : IRequest<Result<Guid>>;

public record OrderLineItem(Guid ProductId, int Quantity);
