using MediatR;
using BubbleShop.Application.Common.Interfaces;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.AIAgent.Commands.ProcessAgentTurn;

public record ProcessAgentTurnCommand(
    string WhatsAppNumber,
    string NewMessage) : IRequest<Result<AgentTurnResponse>>;

public record AgentTurnResponse(string TextReply, List<ToolCall> ToolCalls);
