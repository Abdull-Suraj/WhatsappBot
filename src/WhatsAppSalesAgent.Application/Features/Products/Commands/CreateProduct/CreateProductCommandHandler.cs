using MediatR;
using WhatsAppSalesAgent.Application.Common.Interfaces;
using WhatsAppSalesAgent.Application.Common.Models;
using WhatsAppSalesAgent.Domain.Entities;

namespace WhatsAppSalesAgent.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(
            request.Name,
            request.Description,
            request.Price,
            request.StockQuantity,
            request.ImageUrl);

        await _unitOfWork.Products.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(product.Id);
    }
}
