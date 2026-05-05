using MediatR;
using WhatsAppSalesAgent.Application.Common.Interfaces;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.AIAgent.Commands.ProcessAgentTurn;

public class ProcessAgentTurnCommandHandler : IRequestHandler<ProcessAgentTurnCommand, Result<AgentTurnResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAIAgentService _aiAgentService;

    public ProcessAgentTurnCommandHandler(IUnitOfWork unitOfWork, IAIAgentService aiAgentService)
    {
        _unitOfWork = unitOfWork;
        _aiAgentService = aiAgentService;
    }

    public async Task<Result<AgentTurnResponse>> Handle(ProcessAgentTurnCommand request, CancellationToken cancellationToken)
    {
        var conversation = await _unitOfWork.Conversations.GetByWhatsAppNumberAsync(request.WhatsAppNumber, cancellationToken);

        var history = conversation?.MessageHistory.ToList() ?? new();

        var agentResponse = await _aiAgentService.ProcessAsync(
            history,
            request.NewMessage,
            conversation?.CustomerId.ToString() ?? string.Empty,
            cancellationToken);

        return Result<AgentTurnResponse>.Success(
            new AgentTurnResponse(agentResponse.TextReply, agentResponse.ToolCalls));
    }
}
