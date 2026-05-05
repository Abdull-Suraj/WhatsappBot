using MediatR;
using WhatsAppSalesAgent.Application.Common.Interfaces;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.WhatsApp.Commands.SendMessage;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Result>
{
    private readonly IWhatsAppService _whatsAppService;

    public SendMessageCommandHandler(IWhatsAppService whatsAppService) => _whatsAppService = whatsAppService;

    public async Task<Result> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        await _whatsAppService.SendTextMessageAsync(request.ToNumber, request.Message, cancellationToken);
        return Result.Success();
    }
}
