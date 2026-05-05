using MediatR;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.WhatsApp.Commands.SendMessage;

public record SendMessageCommand(string ToNumber, string Message) : IRequest<Result>;
