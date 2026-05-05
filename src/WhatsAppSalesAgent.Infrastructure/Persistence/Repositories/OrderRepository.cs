using Microsoft.EntityFrameworkCore;
using WhatsAppSalesAgent.Domain.Entities;
using WhatsAppSalesAgent.Domain.Interfaces.Repositories;
using WhatsAppSalesAgent.Infrastructure.Persistence;

namespace WhatsAppSalesAgent.Infrastructure.Persistence.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
        => await _dbSet
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.OrderItems)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task<Order?> GetWithItemsAsync(Guid orderId, CancellationToken cancellationToken = default)
        => await _dbSet
            .Include(o => o.OrderItems)
            .Include(o => o.Payment)
            .Include(o => o.Delivery)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
}
