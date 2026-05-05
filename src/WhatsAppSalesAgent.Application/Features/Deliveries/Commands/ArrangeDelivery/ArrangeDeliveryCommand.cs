using MediatR;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.Deliveries.Commands.ArrangeDelivery;

public record ArrangeDeliveryCommand(
    Guid OrderId,
    string RecipientName,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string Postcode,
    string Country) : IRequest<Result<string>>;
