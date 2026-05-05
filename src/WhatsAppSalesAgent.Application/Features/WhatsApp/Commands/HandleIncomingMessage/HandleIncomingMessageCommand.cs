using MediatR;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.WhatsApp.Commands.HandleIncomingMessage;

public record HandleIncomingMessageCommand(
    string FromNumber,
    string MessageText,
    string? MessageId = null) : IRequest<Result>;
