using BubbleShop.Domain.Common;
using BubbleShop.Domain.Enums;
using BubbleShop.Domain.Events;
using BubbleShop.Domain.Exceptions;

namespace BubbleShop.Domain.Entities;

public class Delivery : BaseEntity
{

    public Guid OrderId { get; private set; }
    public Guid BusinessId { get; private set; }
    public string TrackingNumber { get; private set; }
    public DeliveryStatus Status { get; private set; }
    public DeliveryType DeliveryType { get; private set; }


    // Addresses
    public string PickupAddress { get; private set; }
    public string DeliveryAddress { get; private set; }
    public string CurrentLocation { get; private set; }


    // Time tracking
    public DateTime? EstimatedDeliveryTime { get; private set; }
    public DateTime? ActualPickupTime { get; private set; }
    public DateTime? ActualDeliveryTime { get; private set; }


    // Delivery personnel
    public Guid? DeliveryPersonId { get; private set; }
    public string DeliveryPersonName { get; private set; }
    public string DeliveryPersonPhone { get; private set; }


    // Tracking updates
    private readonly List<DeliveryTracking> _trackingHistory = new();
    public IReadOnlyCollection<DeliveryTracking> TrackingHistory => _trackingHistory.AsReadOnly();

    // Navigation Properties
    public Order Order { get; private set; }
    public Business Business { get; private set; }

    private Delivery() { }

    public  Delivery(
        Guid orderId,
        Guid businessId,
        string deliveryAddress,
        DeliveryType deliveryType = DeliveryType.Standard)
    {
        OrderId = orderId;
        BusinessId = businessId;
        DeliveryAddress = deliveryAddress;
        DeliveryType = deliveryType;
        Status = DeliveryStatus.Pending;
        TrackingNumber = GenerateTrackingNumber();
        SetEstimatedDeliveryTime();

    }
    public void AssignDeliveryPerson(string personName, string personPhone, Guid? personId = null)
    {
        DeliveryPersonName = personName ?? throw new ArgumentNullException(nameof(personName));
        DeliveryPersonPhone = personPhone ?? throw new ArgumentNullException(nameof(personPhone));
        DeliveryPersonId = personId;
        Status = DeliveryStatus.Assigned;
        LastModifiedAt = DateTime.UtcNow;

        AddTrackingUpdate(DeliveryStatus.Assigned, "Delivery person assigned");
    }

    public void MarkAsPickedUp(string location = null)
    {
        if (Status != DeliveryStatus.Assigned)
            throw new DomainException("Delivery must be assigned before pickup");

        Status = DeliveryStatus.PickedUp;
        ActualPickupTime = DateTime.UtcNow;
        CurrentLocation = location ?? PickupAddress;
        LastModifiedAt = DateTime.UtcNow;

        AddTrackingUpdate(DeliveryStatus.PickedUp, "Package picked up");
        AddDomainEvent(new DeliveryPickedUpEvent(Id, OrderId, TrackingNumber));
    }

    public void UpdateLocation(string location, string notes = null)
    {
        CurrentLocation = location;
        LastModifiedAt = DateTime.UtcNow;

        AddTrackingUpdate(DeliveryStatus.InTransit, $"Current location: {location}", notes);
    }

    public void MarkAsOutForDelivery()
    {
        if (Status != DeliveryStatus.PickedUp)
            throw new DomainException("Delivery must be picked up before being out for delivery");

        Status = DeliveryStatus.OutForDelivery;
        LastModifiedAt = DateTime.UtcNow;

        AddTrackingUpdate(DeliveryStatus.OutForDelivery, "Package is out for delivery");
    }

    public void MarkAsDelivered()
    {
        if (Status != DeliveryStatus.OutForDelivery)
            throw new DomainException("Delivery must be out for delivery before completion");

        Status = DeliveryStatus.Delivered;
        ActualDeliveryTime = DateTime.UtcNow;
        LastModifiedAt = DateTime.UtcNow;

        AddTrackingUpdate(DeliveryStatus.Delivered, "Package delivered successfully");
        AddDomainEvent(new DeliveryCompletedEvent(Id, OrderId, TrackingNumber));
    }

    public void MarkAsFailed(string reason)
    {
        Status = DeliveryStatus.Failed;
        LastModifiedAt = DateTime.UtcNow;

        AddTrackingUpdate(DeliveryStatus.Failed, $"Delivery failed: {reason}");
        AddDomainEvent(new DeliveryFailedEvent(Id, OrderId, TrackingNumber, reason));
    }

    public void RescheduleDelivery(DateTime newEstimatedTime, string reason)
    {
        EstimatedDeliveryTime = newEstimatedTime;
        LastModifiedAt = DateTime.UtcNow;

        AddTrackingUpdate(DeliveryStatus.Rescheduled, $"Delivery rescheduled: {reason}", newEstimatedTime.ToString());
        AddDomainEvent(new DeliveryRescheduledEvent(Id, OrderId, TrackingNumber, newEstimatedTime));
    }

    private void SetEstimatedDeliveryTime()
    {
        EstimatedDeliveryTime = DeliveryType switch
        {
            DeliveryType.Express => DateTime.UtcNow.AddHours(2),
            DeliveryType.Standard => DateTime.UtcNow.AddDays(2),
            DeliveryType.Scheduled => DateTime.UtcNow.AddDays(3),
            _ => DateTime.UtcNow.AddDays(3)
        };
    }

    private void AddTrackingUpdate(DeliveryStatus status, string description, string additionalInfo = null)
    {
        _trackingHistory.Add(new DeliveryTracking
        {
            Status = status,
            Location = CurrentLocation,
            Description = description,
            AdditionalInfo = additionalInfo,
            Timestamp = DateTime.UtcNow
        });
    }

    private static string GenerateTrackingNumber()
    {
        return $"TRK-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid():N}"[..10].ToUpper();
    }

    public bool IsDelayed => EstimatedDeliveryTime.HasValue &&
                             EstimatedDeliveryTime.Value < DateTime.UtcNow &&
                             Status != DeliveryStatus.Delivered &&
                             Status != DeliveryStatus.Failed;
}

public enum DeliveryStatus
{
    Pending,
    Assigned,
    PickedUp,
    InTransit,
    OutForDelivery,
    Delivered,
    Failed,
    Rescheduled,
    Cancelled
}

public enum DeliveryType
{
    Standard,
    Express,
    Scheduled,
    Pickup
}

public class DeliveryTracking
{
    public DeliveryStatus Status { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public string AdditionalInfo { get; set; }
    public DateTime Timestamp { get; set; }
}