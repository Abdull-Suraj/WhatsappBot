using MediatR;
using Microsoft.Extensions.Logging;
using BubbleShop.Application.Common.Interfaces;
using BubbleShop.Application.Common.Models;
using BubbleShop.Domain.Entities;
using BubbleShop.Domain.Enums;

namespace BubbleShop.Application.Features.WhatsApp.Commands.HandleIncomingMessage;

public class HandleIncomingMessageCommandHandler : IRequestHandler<HandleIncomingMessageCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWhatsAppService _whatsAppService;
    private readonly IAIAgentService _aiAgentService;
    private readonly ILogger<HandleIncomingMessageCommandHandler> _logger;

    public HandleIncomingMessageCommandHandler(
        IUnitOfWork unitOfWork,
        IWhatsAppService whatsAppService,
        IAIAgentService aiAgentService,
        ILogger<HandleIncomingMessageCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _whatsAppService = whatsAppService;
        _aiAgentService = aiAgentService;
        _logger = logger;
    }

    public async Task<Result> Handle(HandleIncomingMessageCommand request, CancellationToken cancellationToken)
    {
        // Get or create customer
        var customer = await _unitOfWork.Customers.GetByWhatsAppNumberAsync(request.FromNumber, cancellationToken);
        if (customer is null)
        {
            customer = Customer.Create(request.FromNumber, request.FromNumber);
            await _unitOfWork.Customers.AddAsync(customer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Get or create conversation
        var conversation = await _unitOfWork.Conversations.GetByWhatsAppNumberAsync(request.FromNumber, cancellationToken);
        if (conversation is null)
        {
            conversation = Conversation.Create(customer.Id, request.FromNumber);
            await _unitOfWork.Conversations.AddAsync(conversation, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Add the incoming user message
        conversation.AddMessage(ChatRole.User, request.MessageText);

        // Process through AI agent
        var agentResponse = await _aiAgentService.ProcessAsync(
            conversation.MessageHistory.ToList(),
            request.MessageText,
            customer.Id.ToString(),
            cancellationToken);

        // Add assistant reply to history
        if (!string.IsNullOrWhiteSpace(agentResponse.TextReply))
        {
            conversation.AddMessage(ChatRole.Assistant, agentResponse.TextReply);
        }

        // Persist updated conversation history
        await _unitOfWork.Conversations.UpdateMessageHistoryAsync(
            conversation.Id, conversation.MessageHistory, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Send reply via WhatsApp
        if (!string.IsNullOrWhiteSpace(agentResponse.TextReply))
        {
            await _whatsAppService.SendTextMessageAsync(
                request.FromNumber, agentResponse.TextReply, cancellationToken);
        }

        _logger.LogInformation(
            "Processed message from {Number}. AI reply length: {Length}",
            request.FromNumber,
            agentResponse.TextReply?.Length ?? 0);

        return Result.Success();
    }
}
