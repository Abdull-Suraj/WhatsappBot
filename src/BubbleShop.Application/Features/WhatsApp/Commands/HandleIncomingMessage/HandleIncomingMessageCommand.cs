using MediatR;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.WhatsApp.Commands.HandleIncomingMessage;

public record HandleIncomingMessageCommand(
    string FromNumber,
    string MessageText,
    string? MessageId = null) : IRequest<Result>;
