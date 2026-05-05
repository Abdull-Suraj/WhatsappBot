using MediatR;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.Orders.Queries.GetOrdersByCustomer;

public record GetOrdersByCustomerQuery(Guid CustomerId) : IRequest<Result<IReadOnlyList<OrderSummaryDto>>>;

public record OrderSummaryDto(
    Guid Id,
    string StatusName,
    decimal TotalAmount,
    DateTime CreatedAt,
    int ItemCount);
