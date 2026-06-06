using AutoMapper;
using MediatR;
using BubbleShop.Application.Common.Interfaces;
using BubbleShop.Application.Common.Models;
using BubbleShop.Application.Features.Products.Queries.GetAllProducts;

namespace BubbleShop.Application.Features.Products.Queries.SearchProducts;

public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, Result<PagedList<ProductDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SearchProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<ProductDto>>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        var products = string.IsNullOrWhiteSpace(request.Keyword)
            ? await _unitOfWork.Products.GetAvailableAsync(cancellationToken)
            : await _unitOfWork.Products.SearchAsync(request.Keyword, cancellationToken);

        var dtos = _mapper.Map<IReadOnlyList<ProductDto>>(products);
        var paged = PagedList<ProductDto>.Create(dtos, request.PageNumber, request.PageSize);
        return Result<PagedList<ProductDto>>.Success(paged);
    }
}
