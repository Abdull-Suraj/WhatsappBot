using WhatsAppSalesAgent.Domain.Enums;

namespace WhatsAppSalesAgent.Domain.Entities;

public class Conversation
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public string WhatsAppNumber { get; private set; } = string.Empty;
    public List<ChatMessage> MessageHistory { get; private set; } = new();
    public DateTime LastUpdated { get; private set; }

    private Conversation() { }

    public static Conversation Create(Guid customerId, string whatsAppNumber)
    {
        if (customerId == Guid.Empty) throw new ArgumentException("CustomerId cannot be empty.", nameof(customerId));
        ArgumentException.ThrowIfNullOrWhiteSpace(whatsAppNumber);

        return new Conversation
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            WhatsAppNumber = whatsAppNumber,
            LastUpdated = DateTime.UtcNow
        };
    }

    public void AddMessage(ChatRole role, string content)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);
        MessageHistory.Add(new ChatMessage(role, content, DateTime.UtcNow));
        LastUpdated = DateTime.UtcNow;
    }

    public void UpdateHistory(IEnumerable<ChatMessage> messages)
    {
        MessageHistory = messages.ToList();
        LastUpdated = DateTime.UtcNow;
    }
}
