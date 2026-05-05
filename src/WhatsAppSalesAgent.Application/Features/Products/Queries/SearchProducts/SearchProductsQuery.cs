using MediatR;
using WhatsAppSalesAgent.Application.Common.Models;
using WhatsAppSalesAgent.Application.Features.Products.Queries.GetAllProducts;

namespace WhatsAppSalesAgent.Application.Features.Products.Queries.SearchProducts;

public record SearchProductsQuery(string? Keyword, int PageNumber = 1, int PageSize = 20)
    : IRequest<Result<PagedList<ProductDto>>>;
