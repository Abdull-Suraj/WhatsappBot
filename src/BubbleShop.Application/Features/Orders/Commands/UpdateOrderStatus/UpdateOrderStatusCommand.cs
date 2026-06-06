using MediatR;
using BubbleShop.Application.Common.Models;
using BubbleShop.Domain.Enums;

namespace BubbleShop.Application.Features.Orders.Commands.UpdateOrderStatus;

public record UpdateOrderStatusCommand(Guid OrderId, OrderStatus NewStatus) : IRequest<Result>;
