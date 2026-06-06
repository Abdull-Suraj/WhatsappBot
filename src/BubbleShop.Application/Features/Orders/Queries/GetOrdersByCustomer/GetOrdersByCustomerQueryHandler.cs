using MediatR;
using BubbleShop.Application.Common.Interfaces;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.Orders.Queries.GetOrdersByCustomer;

public class GetOrdersByCustomerQueryHandler
    : IRequestHandler<GetOrdersByCustomerQuery, Result<IReadOnlyList<OrderSummaryDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrdersByCustomerQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<OrderSummaryDto>>> Handle(
        GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
    {
        var orders = await _unitOfWork.Orders.GetByCustomerIdAsync(request.CustomerId, cancellationToken);

        var dtos = orders.Select(o => new OrderSummaryDto(
            o.Id,
            o.Status.ToString(),
            o.TotalAmount,
            o.CreatedAt,
            o.OrderItems.Count)).ToList();

        return Result<IReadOnlyList<OrderSummaryDto>>.Success(dtos);
    }
}
