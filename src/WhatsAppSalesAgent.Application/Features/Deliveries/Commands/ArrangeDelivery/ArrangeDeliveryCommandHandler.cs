using MediatR;
using WhatsAppSalesAgent.Application.Common.Interfaces;
using WhatsAppSalesAgent.Application.Common.Models;
using WhatsAppSalesAgent.Domain.Entities;
using WhatsAppSalesAgent.Domain.Enums;

namespace WhatsAppSalesAgent.Application.Features.Deliveries.Commands.ArrangeDelivery;

public class ArrangeDeliveryCommandHandler : IRequestHandler<ArrangeDeliveryCommand, Result<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeliveryService _deliveryService;

    public ArrangeDeliveryCommandHandler(IUnitOfWork unitOfWork, IDeliveryService deliveryService)
    {
        _unitOfWork = unitOfWork;
        _deliveryService = deliveryService;
    }

    public async Task<Result<string>> Handle(ArrangeDeliveryCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
            return Result<string>.Failure($"Order '{request.OrderId}' not found.");

        if (order.Status != OrderStatus.Paid)
            return Result<string>.Failure("Delivery can only be arranged for paid orders.");

        var deliveryRequest = new DeliveryRequest(
            request.OrderId,
            request.RecipientName,
            request.AddressLine1,
            request.AddressLine2,
            request.City,
            request.Postcode,
            request.Country);

        var result = await _deliveryService.ArrangeDeliveryAsync(deliveryRequest, cancellationToken);
        if (!result.IsSuccess)
            return Result<string>.Failure("Failed to arrange delivery with provider.");

        var delivery = Delivery.Create(
            request.OrderId,
            request.RecipientName,
            request.AddressLine1,
            request.AddressLine2,
            request.City,
            request.Postcode,
            request.Country,
            result.Provider);

        delivery.Arrange(result.TrackingNumber);
        order.Dispatch(delivery);

        await _unitOfWork.Orders.UpdateAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success(result.TrackingNumber);
    }
}
