using WhatsAppSalesAgent.Domain.Entities;

namespace WhatsAppSalesAgent.Domain.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<IReadOnlyList<Product>> SearchAsync(string keyword, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetAvailableAsync(CancellationToken cancellationToken = default);
}
