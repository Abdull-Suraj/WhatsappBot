using FluentAssertions;
using BubbleShop.Domain.Entities;
using BubbleShop.Domain.Enums;
using BubbleShop.Domain.Events;
using BubbleShop.Domain.Exceptions;

namespace BubbleShop.Domain.Tests.Entities;

public class OrderTests
{
    private static readonly Guid CustomerId = Guid.NewGuid();

    [Fact]
    public void Create_WithValidCustomerId_ShouldReturnPendingOrder()
    {
        var order = Order.Create(CustomerId);

        order.CustomerId.Should().Be(CustomerId);
        order.Status.Should().Be(OrderStatus.Pending);
        order.TotalAmount.Should().Be(0m);
        order.DomainEvents.Should().ContainSingle(e => e is OrderPlacedEvent);
    }

    [Fact]
    public void Create_WithEmptyCustomerId_ShouldThrowArgumentException()
    {
        var act = () => Order.Create(Guid.Empty);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AddItem_ShouldUpdateTotal()
    {
        var order = Order.Create(CustomerId);
        var product = Product.Create("Widget", "Desc", 10m, 5);

        order.AddItem(product, 3);

        order.TotalAmount.Should().Be(30m);
        order.OrderItems.Should().HaveCount(1);
    }

    [Fact]
    public void AddItem_SameTwice_ShouldAccumulateQuantity()
    {
        var order = Order.Create(CustomerId);
        var product = Product.Create("Widget", "Desc", 10m, 10);

        order.AddItem(product, 2);
        order.AddItem(product, 3);

        order.OrderItems.Should().HaveCount(1);
        order.OrderItems.First().Quantity.Should().Be(5);
        order.TotalAmount.Should().Be(50m);
    }

    [Fact]
    public void Confirm_PendingOrderWithItems_ShouldTransitionToConfirmed()
    {
        var order = Order.Create(CustomerId);
        var product = Product.Create("Widget", "Desc", 10m, 5);
        order.AddItem(product, 1);

        order.Confirm();

        order.Status.Should().Be(OrderStatus.Confirmed);
    }

    [Fact]
    public void Confirm_PendingOrderWithNoItems_ShouldThrowDomainException()
    {
        var order = Order.Create(CustomerId);
        var act = () => order.Confirm();
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Cancel_ConfirmedOrder_ShouldTransitionToCancelled()
    {
        var order = Order.Create(CustomerId);
        var product = Product.Create("Widget", "Desc", 10m, 5);
        order.AddItem(product, 1);
        order.Confirm();

        order.Cancel("Customer request");

        order.Status.Should().Be(OrderStatus.Cancelled);
        order.DomainEvents.Should().Contain(e => e is OrderCancelledEvent);
    }

    [Fact]
    public void Cancel_DeliveredOrder_ShouldThrowInvalidOrderStateException()
    {
        var order = Order.Create(CustomerId);
        var product = Product.Create("Widget", "Desc", 10m, 5);
        order.AddItem(product, 1);
        order.Confirm();

        var payment = Payment.Create(order.Id, "Stripe", 10m);
        payment.Complete("txn_123");
        order.MarkAsPaid(payment);

        var delivery = Delivery.Create(order.Id, "John", "1 Main St", null, "London", "SW1A 1AA", "UK");
        delivery.Arrange("TRACK-001");
        order.Dispatch(delivery);

        order.MarkAsDelivered();

        var act = () => order.Cancel("Too late");
        act.Should().Throw<InvalidOrderStateException>();
    }
}
