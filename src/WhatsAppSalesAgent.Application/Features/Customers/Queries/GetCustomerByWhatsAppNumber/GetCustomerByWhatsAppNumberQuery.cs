using MediatR;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.Customers.Queries.GetCustomerByWhatsAppNumber;

public record GetCustomerByWhatsAppNumberQuery(string WhatsAppNumber) : IRequest<Result<CustomerDto?>>;
