using AutoMapper;
using MediatR;
using BubbleShop.Application.Common.Interfaces;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.Customers.Queries.GetCustomerByWhatsAppNumber;

public class GetCustomerByWhatsAppNumberQueryHandler
    : IRequestHandler<GetCustomerByWhatsAppNumberQuery, Result<CustomerDto?>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCustomerByWhatsAppNumberQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CustomerDto?>> Handle(
        GetCustomerByWhatsAppNumberQuery request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customers.GetByWhatsAppNumberAsync(request.WhatsAppNumber, cancellationToken);
        return Result<CustomerDto?>.Success(customer is null ? null : _mapper.Map<CustomerDto>(customer));
    }
}
