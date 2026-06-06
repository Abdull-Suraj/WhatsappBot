using Microsoft.EntityFrameworkCore;
using BubbleShop.Domain.Entities;
using BubbleShop.Domain.Interfaces.Repositories;
using BubbleShop.Infrastructure.Persistence;

namespace BubbleShop.Infrastructure.Persistence.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context) { }

    public async Task<Customer?> GetByWhatsAppNumberAsync(string whatsAppNumber, CancellationToken cancellationToken = default)
        => await _dbSet
            .FirstOrDefaultAsync(c => c.WhatsAppNumber == whatsAppNumber, cancellationToken);
}
