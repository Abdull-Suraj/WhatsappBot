using MediatR;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid ProductId) : IRequest<Result<ProductDetailDto>>;
