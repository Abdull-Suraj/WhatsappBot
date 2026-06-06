using MediatR;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.Deliveries.Queries.GetDeliveryStatus;

public record GetDeliveryStatusQuery(string TrackingNumber) : IRequest<Result<string>>;
