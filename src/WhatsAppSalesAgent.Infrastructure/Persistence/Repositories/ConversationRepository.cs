using Microsoft.EntityFrameworkCore;
using WhatsAppSalesAgent.Domain.Entities;
using WhatsAppSalesAgent.Domain.Interfaces.Repositories;
using WhatsAppSalesAgent.Infrastructure.Persistence;

namespace WhatsAppSalesAgent.Infrastructure.Persistence.Repositories;

public class ConversationRepository : Repository<Conversation>, IConversationRepository
{
    public ConversationRepository(AppDbContext context) : base(context) { }

    public async Task<Conversation?> GetByWhatsAppNumberAsync(string whatsAppNumber, CancellationToken cancellationToken = default)
        => await _dbSet
            .FirstOrDefaultAsync(c => c.WhatsAppNumber == whatsAppNumber, cancellationToken);

    public async Task UpdateMessageHistoryAsync(Guid conversationId, IEnumerable<ChatMessage> messages, CancellationToken cancellationToken = default)
    {
        var conversation = await GetByIdAsync(conversationId, cancellationToken);
        if (conversation is null) return;

        conversation.UpdateHistory(messages);
        _dbSet.Update(conversation);
    }
}
