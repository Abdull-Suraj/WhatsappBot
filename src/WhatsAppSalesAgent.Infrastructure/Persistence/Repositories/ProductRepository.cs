using Microsoft.EntityFrameworkCore;
using WhatsAppSalesAgent.Domain.Entities;
using WhatsAppSalesAgent.Domain.Interfaces.Repositories;
using WhatsAppSalesAgent.Infrastructure.Persistence;

namespace WhatsAppSalesAgent.Infrastructure.Persistence.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Product>> SearchAsync(string keyword, CancellationToken cancellationToken = default)
    {
        var lower = keyword.ToLowerInvariant();
        return await _dbSet
            .Where(p => p.IsActive &&
                        (p.Name.ToLower().Contains(lower) || p.Description.ToLower().Contains(lower)))
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> GetAvailableAsync(CancellationToken cancellationToken = default)
        => await _dbSet
            .Where(p => p.IsActive && p.StockQuantity > 0)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
}
