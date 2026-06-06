using MediatR;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.Products.Queries.GetAllProducts;

public record GetAllProductsQuery(int PageNumber = 1, int PageSize = 20) 
    : IRequest<Result<PagedList<ProductDto>>>;
