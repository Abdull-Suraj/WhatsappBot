using MediatR;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.Payments.Commands.HandlePaymentWebhook;

public record HandlePaymentWebhookCommand(string Payload, string Signature) : IRequest<Result>;
