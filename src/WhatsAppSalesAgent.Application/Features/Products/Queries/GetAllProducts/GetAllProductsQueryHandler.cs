using AutoMapper;
using MediatR;
using WhatsAppSalesAgent.Application.Common.Interfaces;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Result<PagedList<ProductDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _unitOfWork.Products.GetAllAsync(cancellationToken);
        var dtos = _mapper.Map<IReadOnlyList<ProductDto>>(products);
        var paged = PagedList<ProductDto>.Create(dtos, request.PageNumber, request.PageSize);
        return Result<PagedList<ProductDto>>.Success(paged);
    }
}
