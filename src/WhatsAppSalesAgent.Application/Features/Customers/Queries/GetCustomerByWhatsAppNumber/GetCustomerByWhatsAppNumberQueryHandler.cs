using AutoMapper;
using MediatR;
using WhatsAppSalesAgent.Application.Common.Interfaces;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.Customers.Queries.GetCustomerByWhatsAppNumber;

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
