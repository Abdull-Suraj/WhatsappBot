using MediatR;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.Payments.Commands.HandlePaymentWebhook;

public record HandlePaymentWebhookCommand(string Payload, string Signature) : IRequest<Result>;
