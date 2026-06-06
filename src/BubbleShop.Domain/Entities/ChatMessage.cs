using BubbleShop.Domain.Enums;

namespace BubbleShop.Domain.Entities;

public record ChatMessage(ChatRole Role, string Content, DateTime Timestamp);
