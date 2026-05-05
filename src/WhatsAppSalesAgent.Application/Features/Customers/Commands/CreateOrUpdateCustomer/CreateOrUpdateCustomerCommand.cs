using MediatR;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.Customers.Commands.CreateOrUpdateCustomer;

public record CreateOrUpdateCustomerCommand(
    string WhatsAppNumber,
    string Name,
    string? Email,
    string? Address) : IRequest<Result<Guid>>;
