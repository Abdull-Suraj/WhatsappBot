using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleShop.Domain.Events
{

    public record BusinessVerifiedEvent : INotification
    {
        public Guid BusinessId { get; }
        public string BusinessName { get; }
        public DateTime OccurredOn { get; }

        public BusinessVerifiedEvent(Guid businessId, string businessName)
        {
            BusinessId = businessId;
            BusinessName = businessName;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record BusinessSuspendedEvent : INotification
    {
        public Guid BusinessId { get; }
        public string BusinessName { get; }
        public DateTime OccurredOn { get; }

        public BusinessSuspendedEvent(Guid businessId, string businessName)
        {
            BusinessId = businessId;
            BusinessName = businessName;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record BusinessActivatedEvent : INotification
    {
        public Guid BusinessId { get; }
        public string BusinessName { get; }
        public DateTime OccurredOn { get; }

        public BusinessActivatedEvent(Guid businessId, string businessName)
        {
            BusinessId = businessId;
            BusinessName = businessName;
            OccurredOn = DateTime.UtcNow;
        }
    }



    //   WALLET EVENTS  

    public record WalletCreditedEvent : INotification
    {
        public Guid BusinessId { get; }
        public decimal Amount { get; }
        public decimal NewBalance { get; }
        public string Description { get; }
        public DateTime OccurredOn { get; }

        public WalletCreditedEvent(Guid businessId, decimal amount, decimal newBalance, string description = null)
        {
            BusinessId = businessId;
            Amount = amount;
            NewBalance = newBalance;
            Description = description;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record WalletDebitedEvent : INotification
    {
        public Guid BusinessId { get; }
        public decimal Amount { get; }
        public decimal NewBalance { get; }
        public string Description { get; }
        public DateTime OccurredOn { get; }

        public WalletDebitedEvent(Guid businessId, decimal amount, decimal newBalance, string description = null)
        {
            BusinessId = businessId;
            Amount = amount;
            NewBalance = newBalance;
            Description = description;
            OccurredOn = DateTime.UtcNow;
        }
    }



    //   PRODUCT EVENTS  

    public record LowStockEvent : INotification
    {
        public Guid ProductId { get; }
        public string ProductName { get; }
        public int CurrentStock { get; }
        public DateTime OccurredOn { get; }

        public LowStockEvent(Guid productId, string productName, int currentStock)
        {
            ProductId = productId;
            ProductName = productName;
            CurrentStock = currentStock;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record OutOfStockEvent : INotification
    {
        public Guid ProductId { get; }
        public string ProductName { get; }
        public DateTime OccurredOn { get; }

        public OutOfStockEvent(Guid productId, string productName)
        {
            ProductId = productId;
            ProductName = productName;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record StockReducedEvent : INotification
    {
        public Guid ProductId { get; }
        public string ProductName { get; }
        public int QuantityReduced { get; }
        public int RemainingStock { get; }
        public DateTime OccurredOn { get; }

        public StockReducedEvent(Guid productId, string productName, int quantityReduced, int remainingStock)
        {
            ProductId = productId;
            ProductName = productName;
            QuantityReduced = quantityReduced;
            RemainingStock = remainingStock;
            OccurredOn = DateTime.UtcNow;
        }
    }



    //   ORDER EVENTS  

    public record OrderCreatedEvent : INotification
    {
        public Guid OrderId { get; }
        public string OrderNumber { get; }
        public Guid BusinessId { get; }
        public Guid CustomerId { get; }
        public decimal TotalAmount { get; }
        public DateTime OccurredOn { get; }

        public OrderCreatedEvent(Guid orderId, string orderNumber, Guid businessId, Guid customerId, decimal totalAmount)
        {
            OrderId = orderId;
            OrderNumber = orderNumber;
            BusinessId = businessId;
            CustomerId = customerId;
            TotalAmount = totalAmount;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record OrderPaidEvent : INotification
    {
        public Guid OrderId { get; }
        public string OrderNumber { get; }
        public decimal Amount { get; }
        public DateTime OccurredOn { get; }

        public OrderPaidEvent(Guid orderId, string orderNumber, decimal amount)
        {
            OrderId = orderId;
            OrderNumber = orderNumber;
            Amount = amount;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record OrderConfirmedEvent : INotification
    {
        public Guid OrderId { get; }
        public string OrderNumber { get; }
        public DateTime OccurredOn { get; }

        public OrderConfirmedEvent(Guid orderId, string orderNumber)
        {
            OrderId = orderId;
            OrderNumber = orderNumber;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record OrderProcessingStartedEvent : INotification
    {
        public Guid OrderId { get; }
        public string OrderNumber { get; }
        public DateTime OccurredOn { get; }

        public OrderProcessingStartedEvent(Guid orderId, string orderNumber)
        {
            OrderId = orderId;
            OrderNumber = orderNumber;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record OrderCompletedEvent : INotification
    {
        public Guid OrderId { get; }
        public string OrderNumber { get; }
        public DateTime OccurredOn { get; }

        public OrderCompletedEvent(Guid orderId, string orderNumber)
        {
            OrderId = orderId;
            OrderNumber = orderNumber;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record OrderCancelledEvent : INotification
    {
        public Guid OrderId { get; }
        public string OrderNumber { get; }
        public string Reason { get; }
        public DateTime OccurredOn { get; }

        public OrderCancelledEvent(Guid orderId, string orderNumber, string reason)
        {
            OrderId = orderId;
            OrderNumber = orderNumber;
            Reason = reason;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record PaymentRequestedEvent : INotification
    {
        public Guid OrderId { get; }
        public string OrderNumber { get; }
        public decimal Amount { get; }
        public DateTime OccurredOn { get; }

        public PaymentRequestedEvent(Guid orderId, string orderNumber, decimal amount)
        {
            OrderId = orderId;
            OrderNumber = orderNumber;
            Amount = amount;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record DeliveryAssignedEvent : INotification
    {
        public Guid OrderId { get; }
        public string OrderNumber { get; }
        public Guid DeliveryId { get; }
        public DateTime OccurredOn { get; }

        public DeliveryAssignedEvent(Guid orderId, string orderNumber, Guid deliveryId)
        {
            OrderId = orderId;
            OrderNumber = orderNumber;
            DeliveryId = deliveryId;
            OccurredOn = DateTime.UtcNow;
        }
    }



    //   DELIVERY EVENTS  

    public record DeliveryCreatedEvent : INotification
    {
        public Guid DeliveryId { get; }
        public Guid OrderId { get; }
        public string TrackingNumber { get; }
        public DateTime OccurredOn { get; }

        public DeliveryCreatedEvent(Guid deliveryId, Guid orderId, string trackingNumber)
        {
            DeliveryId = deliveryId;
            OrderId = orderId;
            TrackingNumber = trackingNumber;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record DeliveryPickedUpEvent : INotification
    {
        public Guid DeliveryId { get; }
        public Guid OrderId { get; }
        public string TrackingNumber { get; }
        public DateTime OccurredOn { get; }

        public DeliveryPickedUpEvent(Guid deliveryId, Guid orderId, string trackingNumber)
        {
            DeliveryId = deliveryId;
            OrderId = orderId;
            TrackingNumber = trackingNumber;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record DeliveryCompletedEvent : INotification
    {
        public Guid DeliveryId { get; }
        public Guid OrderId { get; }
        public string TrackingNumber { get; }
        public DateTime OccurredOn { get; }

        public DeliveryCompletedEvent(Guid deliveryId, Guid orderId, string trackingNumber)
        {
            DeliveryId = deliveryId;
            OrderId = orderId;
            TrackingNumber = trackingNumber;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record DeliveryFailedEvent : INotification
    {
        public Guid DeliveryId { get; }
        public Guid OrderId { get; }
        public string TrackingNumber { get; }
        public string Reason { get; }
        public DateTime OccurredOn { get; }

        public DeliveryFailedEvent(Guid deliveryId, Guid orderId, string trackingNumber, string reason)
        {
            DeliveryId = deliveryId;
            OrderId = orderId;
            TrackingNumber = trackingNumber;
            Reason = reason;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record DeliveryRescheduledEvent : INotification
    {
        public Guid DeliveryId { get; }
        public Guid OrderId { get; }
        public string TrackingNumber { get; }
        public DateTime NewEstimatedTime { get; }
        public DateTime OccurredOn { get; }

        public DeliveryRescheduledEvent(Guid deliveryId, Guid orderId, string trackingNumber, DateTime newEstimatedTime)
        {
            DeliveryId = deliveryId;
            OrderId = orderId;
            TrackingNumber = trackingNumber;
            NewEstimatedTime = newEstimatedTime;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record DeliveryTrackingUpdatedEvent : INotification
    {
        public Guid DeliveryId { get; }
        public Guid OrderId { get; }
        public string TrackingNumber { get; }
        public string CurrentLocation { get; }
        public string Status { get; }
        public DateTime OccurredOn { get; }

        public DeliveryTrackingUpdatedEvent(Guid deliveryId, Guid orderId, string trackingNumber, string currentLocation, string status)
        {
            DeliveryId = deliveryId;
            OrderId = orderId;
            TrackingNumber = trackingNumber;
            CurrentLocation = currentLocation;
            Status = status;
            OccurredOn = DateTime.UtcNow;
        }
    }



    //   PAYMENT EVENTS  

    public record PaymentInitiatedEvent : INotification
    {
        public Guid PaymentId { get; }
        public Guid OrderId { get; }
        public string TransactionReference { get; }
        public decimal Amount { get; }
        public DateTime OccurredOn { get; }

        public PaymentInitiatedEvent(Guid paymentId, Guid orderId, string transactionReference, decimal amount)
        {
            PaymentId = paymentId;
            OrderId = orderId;
            TransactionReference = transactionReference;
            Amount = amount;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record PaymentSuccessfulEvent : INotification
    {
        public Guid PaymentId { get; }
        public Guid OrderId { get; }
        public string TransactionReference { get; }
        public decimal Amount { get; }
        public DateTime OccurredOn { get; }

        public PaymentSuccessfulEvent(Guid paymentId, Guid orderId, string transactionReference, decimal amount)
        {
            PaymentId = paymentId;
            OrderId = orderId;
            TransactionReference = transactionReference;
            Amount = amount;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record PaymentFailedEvent : INotification
    {
        public Guid PaymentId { get; }
        public Guid OrderId { get; }
        public string TransactionReference { get; }
        public string FailureReason { get; }
        public DateTime OccurredOn { get; }

        public PaymentFailedEvent(Guid paymentId, Guid orderId, string transactionReference, string failureReason)
        {
            PaymentId = paymentId;
            OrderId = orderId;
            TransactionReference = transactionReference;
            FailureReason = failureReason;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record PaymentRefundedEvent : INotification
    {
        public Guid PaymentId { get; }
        public Guid OrderId { get; }
        public string TransactionReference { get; }
        public decimal RefundAmount { get; }
        public string Reason { get; }
        public DateTime OccurredOn { get; }

        public PaymentRefundedEvent(Guid paymentId, Guid orderId, string transactionReference, decimal refundAmount, string reason)
        {
            PaymentId = paymentId;
            OrderId = orderId;
            TransactionReference = transactionReference;
            RefundAmount = refundAmount;
            Reason = reason;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record PaymentVoidedEvent : INotification
    {
        public Guid PaymentId { get; }
        public Guid OrderId { get; }
        public string TransactionReference { get; }
        public string Reason { get; }
        public DateTime OccurredOn { get; }

        public PaymentVoidedEvent(Guid paymentId, Guid orderId, string transactionReference, string reason)
        {
            PaymentId = paymentId;
            OrderId = orderId;
            TransactionReference = transactionReference;
            Reason = reason;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record PartialPaymentReceivedEvent : INotification
    {
        public Guid PaymentId { get; }
        public Guid OrderId { get; }
        public string TransactionReference { get; }
        public decimal AmountReceived { get; }
        public decimal TotalPaid { get; }
        public DateTime OccurredOn { get; }

        public PartialPaymentReceivedEvent(Guid paymentId, Guid orderId, string transactionReference, decimal amountReceived, decimal totalPaid)
        {
            PaymentId = paymentId;
            OrderId = orderId;
            TransactionReference = transactionReference;
            AmountReceived = amountReceived;
            TotalPaid = totalPaid;
            OccurredOn = DateTime.UtcNow;
        }
    }



    //   CUSTOMER EVENTS  

    public record CustomerRegisteredEvent : INotification
    {
        public Guid CustomerId { get; }
        public string FullName { get; }
        public string WhatsAppNumber { get; }
        public Guid BusinessId { get; }
        public DateTime OccurredOn { get; }

        public CustomerRegisteredEvent(Guid customerId, string fullName, string whatsAppNumber, Guid businessId)
        {
            CustomerId = customerId;
            FullName = fullName;
            WhatsAppNumber = whatsAppNumber;
            BusinessId = businessId;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record CustomerBlockedEvent : INotification
    {
        public Guid CustomerId { get; }
        public string FullName { get; }
        public string Reason { get; }
        public DateTime OccurredOn { get; }

        public CustomerBlockedEvent(Guid customerId, string fullName, string reason = null)
        {
            CustomerId = customerId;
            FullName = fullName;
            Reason = reason;
            OccurredOn = DateTime.UtcNow;
        }
    }


    //   WHATSAPP EVENTS  

    public record WhatsAppMessageReceivedEvent : INotification
    {
        public string From { get; }
        public string To { get; }
        public string Message { get; }
        public DateTime OccurredOn { get; }

        public WhatsAppMessageReceivedEvent(string from, string to, string message)
        {
            From = from;
            To = to;
            Message = message;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record WhatsAppMessageSentEvent : INotification
    {
        public string To { get; }
        public string Message { get; }
        public bool Success { get; }
        public DateTime OccurredOn { get; }

        public WhatsAppMessageSentEvent(string to, string message, bool success)
        {
            To = to;
            Message = message;
            Success = success;
            OccurredOn = DateTime.UtcNow;
        }
    }



    //   NOTIFICATION EVENTS 

    public record OrderStatusChangedNotification : INotification
    {
        public Guid OrderId { get; }
        public string OrderNumber { get; }
        public string OldStatus { get; }
        public string NewStatus { get; }
        public string CustomerWhatsApp { get; }
        public DateTime OccurredOn { get; }

        public OrderStatusChangedNotification(Guid orderId, string orderNumber, string oldStatus, string newStatus, string customerWhatsApp)
        {
            OrderId = orderId;
            OrderNumber = orderNumber;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            CustomerWhatsApp = customerWhatsApp;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public record DeliveryStatusChangedNotification : INotification
    {
        public Guid DeliveryId { get; }
        public string TrackingNumber { get; }
        public string OldStatus { get; }
        public string NewStatus { get; }
        public string CustomerWhatsApp { get; }
        public DateTime OccurredOn { get; }

        public DeliveryStatusChangedNotification(Guid deliveryId, string trackingNumber, string oldStatus, string newStatus, string customerWhatsApp)
        {
            DeliveryId = deliveryId;
            TrackingNumber = trackingNumber;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            CustomerWhatsApp = customerWhatsApp;
            OccurredOn = DateTime.UtcNow;
        }
    }

}