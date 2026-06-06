using BubbleShop.Domain.Entities;

namespace BubbleShop.Domain.Interfaces.Repositories;

public interface IConversationRepository : IRepository<Conversation>
{
    Task<Conversation?> GetByWhatsAppNumberAsync(string whatsAppNumber, CancellationToken cancellationToken = default);
    Task UpdateMessageHistoryAsync(Guid conversationId, IEnumerable<ChatMessage> messages, CancellationToken cancellationToken = default);
}
