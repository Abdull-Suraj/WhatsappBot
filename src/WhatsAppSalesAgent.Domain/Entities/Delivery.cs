using WhatsAppSalesAgent.Domain.Enums;

namespace WhatsAppSalesAgent.Domain.Entities;

public class Delivery
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public string RecipientName { get; private set; } = string.Empty;
    public string AddressLine1 { get; private set; } = string.Empty;
    public string? AddressLine2 { get; private set; }
    public string City { get; private set; } = string.Empty;
    public string Postcode { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public string? TrackingNumber { get; private set; }
    public DeliveryStatus Status { get; private set; }
    public string? Provider { get; private set; }

    private Delivery() { }

    public static Delivery Create(
        Guid orderId,
        string recipientName,
        string addressLine1,
        string? addressLine2,
        string city,
        string postcode,
        string country,
        string? provider = null)
    {
        if (orderId == Guid.Empty) throw new ArgumentException("OrderId cannot be empty.", nameof(orderId));
        ArgumentException.ThrowIfNullOrWhiteSpace(recipientName);
        ArgumentException.ThrowIfNullOrWhiteSpace(addressLine1);
        ArgumentException.ThrowIfNullOrWhiteSpace(city);
        ArgumentException.ThrowIfNullOrWhiteSpace(postcode);
        ArgumentException.ThrowIfNullOrWhiteSpace(country);

        return new Delivery
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            RecipientName = recipientName,
            AddressLine1 = addressLine1,
            AddressLine2 = addressLine2,
            City = city,
            Postcode = postcode,
            Country = country,
            Provider = provider,
            Status = DeliveryStatus.Pending
        };
    }

    public void Arrange(string trackingNumber)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(trackingNumber);
        TrackingNumber = trackingNumber;
        Status = DeliveryStatus.Arranged;
    }

    public void MarkInTransit() => Status = DeliveryStatus.InTransit;

    public void MarkDelivered() => Status = DeliveryStatus.Delivered;
}
