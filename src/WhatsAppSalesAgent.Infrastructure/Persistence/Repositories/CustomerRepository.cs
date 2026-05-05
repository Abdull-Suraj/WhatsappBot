using Microsoft.EntityFrameworkCore;
using WhatsAppSalesAgent.Domain.Entities;
using WhatsAppSalesAgent.Domain.Interfaces.Repositories;
using WhatsAppSalesAgent.Infrastructure.Persistence;

namespace WhatsAppSalesAgent.Infrastructure.Persistence.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context) { }

    public async Task<Customer?> GetByWhatsAppNumberAsync(string whatsAppNumber, CancellationToken cancellationToken = default)
        => await _dbSet
            .FirstOrDefaultAsync(c => c.WhatsAppNumber == whatsAppNumber, cancellationToken);
}
