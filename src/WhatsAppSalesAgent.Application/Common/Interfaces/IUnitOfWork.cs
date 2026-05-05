using WhatsAppSalesAgent.Domain.Interfaces.Repositories;

namespace WhatsAppSalesAgent.Application.Common.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    ICustomerRepository Customers { get; }
    IConversationRepository Conversations { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
