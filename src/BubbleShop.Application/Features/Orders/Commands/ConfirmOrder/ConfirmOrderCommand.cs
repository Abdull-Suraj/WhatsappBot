using MediatR;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.Orders.Commands.ConfirmOrder;

public record ConfirmOrderCommand(Guid OrderId) : IRequest<Result>;
