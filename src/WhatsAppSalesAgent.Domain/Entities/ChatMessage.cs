using WhatsAppSalesAgent.Domain.Enums;

namespace WhatsAppSalesAgent.Domain.Entities;

public record ChatMessage(ChatRole Role, string Content, DateTime Timestamp);
