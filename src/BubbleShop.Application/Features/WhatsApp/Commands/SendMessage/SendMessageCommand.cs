using MediatR;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.WhatsApp.Commands.SendMessage;

public record SendMessageCommand(string ToNumber, string Message) : IRequest<Result>;
