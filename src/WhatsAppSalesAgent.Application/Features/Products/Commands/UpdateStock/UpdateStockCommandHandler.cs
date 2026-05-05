using MediatR;
using WhatsAppSalesAgent.Application.Common.Interfaces;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Features.Products.Commands.UpdateStock;

public class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStockCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        if (request.NewQuantity < 0)
            return Result.Failure("Stock quantity cannot be negative.");

        var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            return Result.Failure($"Product '{request.ProductId}' not found.");

        product.UpdateStock(request.NewQuantity);
        await _unitOfWork.Products.UpdateAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
