using MediatR;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.Payments.Commands.InitiatePayment;

public record InitiatePaymentCommand(Guid OrderId, string Currency = "GBP") : IRequest<Result<string>>;
