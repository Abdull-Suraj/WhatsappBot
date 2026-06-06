using MediatR;
using BubbleShop.Application.Common.Interfaces;
using BubbleShop.Application.Common.Models;
using BubbleShop.Domain.Entities;
using BubbleShop.Domain.Exceptions;

namespace BubbleShop.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer is null)
            return Result<Guid>.Failure($"Customer '{request.CustomerId}' not found.");

        var order = Order.Create(request.CustomerId);

        foreach (var line in request.Items)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(line.ProductId, cancellationToken);
            if (product is null)
                return Result<Guid>.Failure($"Product '{line.ProductId}' not found.");

            if (!product.IsActive)
                return Result<Guid>.Failure($"Product '{product.Name}' is not available.");

            if (product.StockQuantity < line.Quantity)
                return Result<Guid>.Failure(
                    $"Insufficient stock for '{product.Name}'. Available: {product.StockQuantity}, requested: {line.Quantity}.");

            order.AddItem(product, line.Quantity);
            product.DeductStock(line.Quantity);
            await _unitOfWork.Products.UpdateAsync(product, cancellationToken);
        }

        await _unitOfWork.Orders.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(order.Id);
    }
}
