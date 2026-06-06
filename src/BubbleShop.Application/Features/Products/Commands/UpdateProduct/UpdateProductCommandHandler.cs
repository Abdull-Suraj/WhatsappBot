using MediatR;
using BubbleShop.Application.Common.Interfaces;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            return Result.Failure($"Product '{request.ProductId}' not found.");

        product.Update(request.Name, request.Description, request.Price, request.ImageUrl);

        if (request.IsActive) product.Activate();
        else product.Deactivate();

        await _unitOfWork.Products.UpdateAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
