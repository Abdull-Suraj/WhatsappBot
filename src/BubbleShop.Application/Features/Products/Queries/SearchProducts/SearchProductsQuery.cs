using MediatR;
using BubbleShop.Application.Common.Models;
using BubbleShop.Application.Features.Products.Queries.GetAllProducts;

namespace BubbleShop.Application.Features.Products.Queries.SearchProducts;

public record SearchProductsQuery(string? Keyword, int PageNumber = 1, int PageSize = 20)
    : IRequest<Result<PagedList<ProductDto>>>;
