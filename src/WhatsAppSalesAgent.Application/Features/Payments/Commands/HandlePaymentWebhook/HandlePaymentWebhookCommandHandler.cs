using MediatR;
using Microsoft.Extensions.Logging;
using WhatsAppSalesAgent.Application.Common.Interfaces;
using WhatsAppSalesAgent.Application.Common.Models;
using WhatsAppSalesAgent.Domain.Entities;

namespace WhatsAppSalesAgent.Application.Features.Payments.Commands.HandlePaymentWebhook;

public class HandlePaymentWebhookCommandHandler : IRequestHandler<HandlePaymentWebhookCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentService _paymentService;
    private readonly ILogger<HandlePaymentWebhookCommandHandler> _logger;

    public HandlePaymentWebhookCommandHandler(
        IUnitOfWork unitOfWork,
        IPaymentService paymentService,
        ILogger<HandlePaymentWebhookCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _paymentService = paymentService;
        _logger = logger;
    }

    public async Task<Result> Handle(HandlePaymentWebhookCommand request, CancellationToken cancellationToken)
    {
        var webhookResult = await _paymentService.HandleWebhookAsync(request.Payload, request.Signature, cancellationToken);

        if (!webhookResult.IsSuccess)
        {
            _logger.LogWarning("Payment webhook processing failed for order {OrderId}", webhookResult.OrderId);
            return Result.Failure("Payment webhook processing failed.");
        }

        var order = await _unitOfWork.Orders.GetWithItemsAsync(webhookResult.OrderId, cancellationToken);
        if (order is null)
            return Result.Failure($"Order '{webhookResult.OrderId}' not found.");

        var payment = Payment.Create(order.Id, "Stripe", webhookResult.Amount);
        payment.Complete(webhookResult.TransactionId);
        order.MarkAsPaid(payment);

        await _unitOfWork.Orders.UpdateAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Payment completed for order {OrderId}, transaction {TransactionId}",
            order.Id, webhookResult.TransactionId);

        return Result.Success();
    }
}
