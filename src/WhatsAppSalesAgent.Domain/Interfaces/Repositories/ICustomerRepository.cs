using WhatsAppSalesAgent.Domain.Entities;

namespace WhatsAppSalesAgent.Domain.Interfaces.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByWhatsAppNumberAsync(string whatsAppNumber, CancellationToken cancellationToken = default);
}
