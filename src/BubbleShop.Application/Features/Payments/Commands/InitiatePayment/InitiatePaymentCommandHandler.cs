using MediatR;
using BubbleShop.Application.Common.Interfaces;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.Payments.Commands.InitiatePayment;

public class InitiatePaymentCommandHandler : IRequestHandler<InitiatePaymentCommand, Result<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentService _paymentService;

    public InitiatePaymentCommandHandler(IUnitOfWork unitOfWork, IPaymentService paymentService)
    {
        _unitOfWork = unitOfWork;
        _paymentService = paymentService;
    }

    public async Task<Result<string>> Handle(InitiatePaymentCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Orders.GetWithItemsAsync(request.OrderId, cancellationToken);
        if (order is null)
            return Result<string>.Failure($"Order '{request.OrderId}' not found.");

        if (order.TotalAmount <= 0)
            return Result<string>.Failure("Order total must be greater than zero.");

        var paymentLink = await _paymentService.CreatePaymentLinkAsync(
            order.Id, order.TotalAmount, request.Currency, cancellationToken);

        return Result<string>.Success(paymentLink);
    }
}
