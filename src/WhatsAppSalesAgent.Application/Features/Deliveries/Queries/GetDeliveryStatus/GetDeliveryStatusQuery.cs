using MediatR;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.Deliveries.Queries.GetDeliveryStatus;

public record GetDeliveryStatusQuery(string TrackingNumber) : IRequest<Result<string>>;
