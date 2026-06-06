namespace BubbleShop.Application.Common.Interfaces;

public interface IPaymentService
{
    Task<string> CreatePaymentLinkAsync(Guid orderId, decimal amount, string currency, CancellationToken cancellationToken = default);
    Task<PaymentWebhookResult> HandleWebhookAsync(string payload, string signature, CancellationToken cancellationToken = default);
}

public record PaymentWebhookResult(bool IsSuccess, string TransactionId, Guid OrderId, decimal Amount);
