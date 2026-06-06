using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BubbleShop.Application.Common.Interfaces;

namespace BubbleShop.Infrastructure.Services;

public class StripePaymentService : IPaymentService
{
    private readonly ILogger<StripePaymentService> _logger;
    private readonly StripeOptions _options;

    public StripePaymentService(IOptions<StripeOptions> options, ILogger<StripePaymentService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> CreatePaymentLinkAsync(Guid orderId, decimal amount, string currency, CancellationToken cancellationToken = default)
    {
        // TODO: Use Stripe.net SDK to create a payment session
        _logger.LogInformation("[Stripe STUB] Creating payment link for order {OrderId}, amount {Amount} {Currency}", orderId, amount, currency);
        await Task.CompletedTask;

        return $"https://checkout.stripe.com/pay/stub_{orderId}";
    }

    public async Task<PaymentWebhookResult> HandleWebhookAsync(string payload, string signature, CancellationToken cancellationToken = default)
    {
        // TODO: Verify Stripe webhook signature and parse event
        _logger.LogInformation("[Stripe STUB] Handling webhook");
        await Task.CompletedTask;

        return new PaymentWebhookResult(false, string.Empty, Guid.Empty, 0m);
    }
}
