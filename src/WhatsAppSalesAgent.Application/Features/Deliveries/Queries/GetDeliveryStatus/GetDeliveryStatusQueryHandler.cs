using MediatR;
using WhatsAppSalesAgent.Application.Common.Interfaces;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.Deliveries.Queries.GetDeliveryStatus;

public class GetDeliveryStatusQueryHandler : IRequestHandler<GetDeliveryStatusQuery, Result<string>>
{
    private readonly IDeliveryService _deliveryService;

    public GetDeliveryStatusQueryHandler(IDeliveryService deliveryService) => _deliveryService = deliveryService;

    public async Task<Result<string>> Handle(GetDeliveryStatusQuery request, CancellationToken cancellationToken)
    {
        var status = await _deliveryService.GetDeliveryStatusAsync(request.TrackingNumber, cancellationToken);
        return Result<string>.Success(status);
    }
}
