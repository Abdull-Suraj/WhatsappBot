using MediatR;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.Deliveries.Commands.ArrangeDelivery;

public record ArrangeDeliveryCommand(
    Guid OrderId,
    string RecipientName,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string Postcode,
    string Country) : IRequest<Result<string>>;
