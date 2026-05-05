using MediatR;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.Products.Queries.GetAllProducts;

public record GetAllProductsQuery(int PageNumber = 1, int PageSize = 20) 
    : IRequest<Result<PagedList<ProductDto>>>;
