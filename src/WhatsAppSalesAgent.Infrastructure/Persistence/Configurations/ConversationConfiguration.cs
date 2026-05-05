using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhatsAppSalesAgent.Domain.Entities;

namespace WhatsAppSalesAgent.Infrastructure.Persistence.Configurations;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.WhatsAppNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(c => c.WhatsAppNumber)
            .IsUnique();

        builder.Property(c => c.MessageHistory)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<ChatMessage>>(v, (JsonSerializerOptions?)null) ?? new List<ChatMessage>())
            .HasColumnType("nvarchar(max)");
    }
}
