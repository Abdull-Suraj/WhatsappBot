using MediatR;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.Payments.Commands.InitiatePayment;

public record InitiatePaymentCommand(Guid OrderId, string Currency = "GBP") : IRequest<Result<string>>;
