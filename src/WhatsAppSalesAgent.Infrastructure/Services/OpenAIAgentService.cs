using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WhatsAppSalesAgent.Application.Common.Interfaces;
using WhatsAppSalesAgent.Domain.Entities;
using WhatsAppSalesAgent.Domain.Enums;

namespace WhatsAppSalesAgent.Infrastructure.Services;

public class OpenAIAgentService : IAIAgentService
{
    private readonly ILogger<OpenAIAgentService> _logger;
    private readonly OpenAIOptions _options;

    public OpenAIAgentService(IOptions<OpenAIOptions> options, ILogger<OpenAIAgentService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task<AgentResponse> ProcessAsync(
        List<ChatMessage> history,
        string newMessage,
        string customerId,
        CancellationToken cancellationToken = default)
    {
        // TODO: Replace stub with actual OpenAI (or Azure OpenAI) chat completions API call.
        // Include tool definitions for: search_products, create_order, get_order_status,
        //   initiate_payment, get_delivery_status, etc.
        _logger.LogInformation(
            "[AI STUB] Processing message for customer {CustomerId}. History length: {Length}",
            customerId, history.Count);

        await Task.CompletedTask;

        var updatedHistory = new List<ChatMessage>(history)
        {
            new ChatMessage(ChatRole.User, newMessage, DateTime.UtcNow)
        };

        var reply = $"Hello! I received your message: '{newMessage}'. (AI stub — integrate OpenAI here.)";
        updatedHistory.Add(new ChatMessage(ChatRole.Assistant, reply, DateTime.UtcNow));

        return new AgentResponse
        {
            TextReply = reply,
            ToolCalls = new List<ToolCall>(),
            UpdatedHistory = updatedHistory
        };
    }
}
