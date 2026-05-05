using Microsoft.Extensions.Logging;
using WhatsAppSalesAgent.Application.Common.Interfaces;

namespace WhatsAppSalesAgent.Infrastructure.Services;

public class DeliveryService : IDeliveryService
{
    private readonly ILogger<DeliveryService> _logger;

    public DeliveryService(ILogger<DeliveryService> logger) => _logger = logger;

    public async Task<DeliveryArrangementResult> ArrangeDeliveryAsync(DeliveryRequest request, CancellationToken cancellationToken = default)
    {
        // TODO: Integrate with a real courier API (Royal Mail, FedEx, DHL, etc.)
        _logger.LogInformation("[Delivery STUB] Arranging delivery for order {OrderId}", request.OrderId);
        await Task.CompletedTask;

        var trackingNumber = $"TRACK-{request.OrderId:N}";
        return new DeliveryArrangementResult(true, trackingNumber, "StubCourier");
    }

    public async Task<string> GetDeliveryStatusAsync(string trackingNumber, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("[Delivery STUB] Getting status for tracking {TrackingNumber}", trackingNumber);
        await Task.CompletedTask;

        return "In Transit (stub)";
    }
}
