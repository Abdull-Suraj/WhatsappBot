using MediatR;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.Orders.Commands.CreateOrder;

public record CreateOrderCommand(
    Guid CustomerId,
    List<OrderLineItem> Items) : IRequest<Result<Guid>>;

public record OrderLineItem(Guid ProductId, int Quantity);
