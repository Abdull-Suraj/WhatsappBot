using MediatR;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.Customers.Queries.GetCustomerByWhatsAppNumber;

public record GetCustomerByWhatsAppNumberQuery(string WhatsAppNumber) : IRequest<Result<CustomerDto?>>;
