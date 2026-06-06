namespace BubbleShop.Application.Common.Interfaces;

public interface IDeliveryService
{
    Task<DeliveryArrangementResult> ArrangeDeliveryAsync(DeliveryRequest request, CancellationToken cancellationToken = default);
    Task<string> GetDeliveryStatusAsync(string trackingNumber, CancellationToken cancellationToken = default);
}

public record DeliveryRequest(
    Guid OrderId,
    string RecipientName,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string Postcode,
    string Country);

public record DeliveryArrangementResult(bool IsSuccess, string TrackingNumber, string Provider);
