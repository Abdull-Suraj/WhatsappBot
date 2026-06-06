namespace BubbleShop.Application.Features.Customers.Queries.GetCustomerByWhatsAppNumber;

public record CustomerDto(
    Guid Id,
    string WhatsAppNumber,
    string Name,
    string? Email,
    string? Address,
    DateTime CreatedAt);
