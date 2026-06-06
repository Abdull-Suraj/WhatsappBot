using BubbleShop.Domain.Entities;

namespace BubbleShop.Domain.Interfaces.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByWhatsAppNumberAsync(string whatsAppNumber, CancellationToken cancellationToken = default);
}
