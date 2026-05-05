using FluentAssertions;
using WhatsAppSalesAgent.Domain.Entities;
using WhatsAppSalesAgent.Domain.Exceptions;

namespace WhatsAppSalesAgent.Domain.Tests.Entities;

public class ProductTests
{
    [Fact]
    public void Create_WithValidData_ShouldReturnActiveProduct()
    {
        var product = Product.Create("Widget", "A great widget", 9.99m, 50);

        product.Name.Should().Be("Widget");
        product.Price.Should().Be(9.99m);
        product.StockQuantity.Should().Be(50);
        product.IsActive.Should().BeTrue();
        product.Id.Should().NotBe(Guid.Empty);
    }

    [Theory]
    [InlineData("", "desc", 10, 5)]
    [InlineData("name", "", 10, 5)]
    public void Create_WithEmptyNameOrDescription_ShouldThrowArgumentException(
        string name, string description, decimal price, int stock)
    {
        var act = () => Product.Create(name, description, price, stock);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithNegativePrice_ShouldThrowArgumentOutOfRangeException()
    {
        var act = () => Product.Create("Widget", "Desc", -1m, 10);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void DeductStock_WithSufficientStock_ShouldReduceQuantity()
    {
        var product = Product.Create("Widget", "Desc", 10m, 20);
        product.DeductStock(5);
        product.StockQuantity.Should().Be(15);
    }

    [Fact]
    public void DeductStock_WithInsufficientStock_ShouldThrowProductOutOfStockException()
    {
        var product = Product.Create("Widget", "Desc", 10m, 3);
        var act = () => product.DeductStock(10);
        act.Should().Throw<ProductOutOfStockException>()
            .Which.RequestedQuantity.Should().Be(10);
    }

    [Fact]
    public void Deactivate_ShouldMarkProductAsInactive()
    {
        var product = Product.Create("Widget", "Desc", 10m, 5);
        product.Deactivate();
        product.IsActive.Should().BeFalse();
    }
}
