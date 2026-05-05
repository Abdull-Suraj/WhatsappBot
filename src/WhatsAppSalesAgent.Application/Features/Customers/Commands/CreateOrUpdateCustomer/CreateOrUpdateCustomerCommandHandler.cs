using MediatR;
using WhatsAppSalesAgent.Application.Common.Interfaces;
using WhatsAppSalesAgent.Application.Common.Models;
using WhatsAppSalesAgent.Domain.Entities;

namespace WhatsAppSalesAgent.Application.Features.Customers.Commands.CreateOrUpdateCustomer;

public class CreateOrUpdateCustomerCommandHandler : IRequestHandler<CreateOrUpdateCustomerCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrUpdateCustomerCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(CreateOrUpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var existing = await _unitOfWork.Customers.GetByWhatsAppNumberAsync(request.WhatsAppNumber, cancellationToken);

        if (existing is not null)
        {
            existing.Update(request.Name, request.Email, request.Address);
            await _unitOfWork.Customers.UpdateAsync(existing, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(existing.Id);
        }

        var customer = Customer.Create(request.WhatsAppNumber, request.Name, request.Email, request.Address);
        await _unitOfWork.Customers.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(customer.Id);
    }
}
