using MediatR;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<Result<OrderDto>>;
