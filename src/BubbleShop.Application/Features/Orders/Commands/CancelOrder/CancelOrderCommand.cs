using MediatR;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.Orders.Commands.CancelOrder;

public record CancelOrderCommand(Guid OrderId, string Reason) : IRequest<Result>;
