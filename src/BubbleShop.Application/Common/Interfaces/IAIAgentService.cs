using BubbleShop.Domain.Entities;

namespace BubbleShop.Application.Common.Interfaces;

public interface IAIAgentService
{
    Task<AgentResponse> ProcessAsync(
        List<ChatMessage> history,
        string newMessage,
        string customerId,
        CancellationToken cancellationToken = default);
}

public class AgentResponse
{
    public string TextReply { get; set; } = string.Empty;
    public List<ToolCall> ToolCalls { get; set; } = new();
    public List<ChatMessage> UpdatedHistory { get; set; } = new();
}

public class ToolCall
{
    public string FunctionName { get; set; } = string.Empty;
    public Dictionary<string, object> Arguments { get; set; } = new();
}
