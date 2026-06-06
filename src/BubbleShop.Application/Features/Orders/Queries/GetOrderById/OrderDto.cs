namespace BubbleShop.Application.Features.Orders.Queries.GetOrderById;

public record OrderDto(
    Guid Id,
    Guid CustomerId,
    string StatusName,
    decimal TotalAmount,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IReadOnlyList<OrderItemDto> OrderItems,
    PaymentDto? Payment,
    DeliveryDto? Delivery);

public record OrderItemDto(
    Guid Id,
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal);

public record PaymentDto(
    Guid Id,
    string Provider,
    string StatusName,
    string? TransactionId,
    decimal Amount,
    DateTime? PaidAt);

public record DeliveryDto(
    Guid Id,
    string RecipientName,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string Postcode,
    string Country,
    string? TrackingNumber,
    string StatusName,
    string? Provider);
