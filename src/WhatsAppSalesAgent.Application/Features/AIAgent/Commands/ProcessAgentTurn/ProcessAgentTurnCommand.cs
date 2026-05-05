using MediatR;
using WhatsAppSalesAgent.Application.Common.Interfaces;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.AIAgent.Commands.ProcessAgentTurn;

public record ProcessAgentTurnCommand(
    string WhatsAppNumber,
    string NewMessage) : IRequest<Result<AgentTurnResponse>>;

public record AgentTurnResponse(string TextReply, List<ToolCall> ToolCalls);
