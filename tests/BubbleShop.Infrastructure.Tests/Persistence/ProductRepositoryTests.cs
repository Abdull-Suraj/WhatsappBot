using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using BubbleShop.Domain.Entities;
using BubbleShop.Infrastructure.Persistence;
using BubbleShop.Infrastructure.Persistence.Repositories;

namespace BubbleShop.Infrastructure.Tests.Persistence;

public class ProductRepositoryTests : IAsyncDisposable
{
    private readonly AppDbContext _context;
    private readonly ProductRepository _repository;

    public ProductRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new ProductRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistProduct()
    {
        var product = Product.Create("Widget", "Test widget", 10m, 5);
        await _repository.AddAsync(product);
        await _context.SaveChangesAsync();

        var fetched = await _repository.GetByIdAsync(product.Id);
        fetched.Should().NotBeNull();
        fetched!.Name.Should().Be("Widget");
    }

    [Fact]
    public async Task GetAvailableAsync_ShouldReturnOnlyActiveWithStock()
    {
        var active = Product.Create("Active", "Desc", 10m, 5);
        var inactive = Product.Create("Inactive", "Desc", 10m, 5);
        inactive.Deactivate();
        var outOfStock = Product.Create("OutOfStock", "Desc", 10m, 0);

        await _repository.AddAsync(active);
        await _repository.AddAsync(inactive);
        await _repository.AddAsync(outOfStock);
        await _context.SaveChangesAsync();

        var available = await _repository.GetAvailableAsync();
        available.Should().HaveCount(1);
        available[0].Name.Should().Be("Active");
    }

    [Fact]
    public async Task SearchAsync_ShouldMatchByNameOrDescription()
    {
        var product1 = Product.Create("Blue Widget", "A blue widget", 10m, 5);
        var product2 = Product.Create("Red Gadget", "A blue item", 20m, 3);
        var product3 = Product.Create("Green Tool", "Something else", 15m, 2);

        await _repository.AddAsync(product1);
        await _repository.AddAsync(product2);
        await _repository.AddAsync(product3);
        await _context.SaveChangesAsync();

        var results = await _repository.SearchAsync("blue");
        results.Should().HaveCount(2);
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}
