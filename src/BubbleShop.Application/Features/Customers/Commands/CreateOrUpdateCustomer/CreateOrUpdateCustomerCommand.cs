using MediatR;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.Customers.Commands.CreateOrUpdateCustomer;

public record CreateOrUpdateCustomerCommand(
    string WhatsAppNumber,
    string Name,
    string? Email,
    string? Address) : IRequest<Result<Guid>>;
