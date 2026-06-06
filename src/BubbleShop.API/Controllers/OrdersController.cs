using Microsoft.AspNetCore.Mvc;
using BubbleShop.Application.Features.Orders.Commands.CancelOrder;
using BubbleShop.Application.Features.Orders.Commands.ConfirmOrder;
using BubbleShop.Application.Features.Orders.Commands.CreateOrder;
using BubbleShop.Application.Features.Orders.Commands.UpdateOrderStatus;
using BubbleShop.Application.Features.Orders.Queries.GetOrderById;
using BubbleShop.Application.Features.Orders.Queries.GetOrdersByCustomer;
using BubbleShop.Domain.Enums;

namespace BubbleShop.API.Controllers;

public class OrdersController : ApiControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetOrderByIdQuery(id), cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("customer/{customerId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCustomer(Guid customerId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetOrdersByCustomerQuery(customerId), cancellationToken);
        return ToActionResult(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(new { errors = result.Errors });

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    [HttpPost("{id:guid}/confirm")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Confirm(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ConfirmOrderCommand(id), cancellationToken);
        return ToActionResult(result);
    }

    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelOrderRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CancelOrderCommand(id, request.Reason), cancellationToken);
        return ToActionResult(result);
    }

    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new UpdateOrderStatusCommand(id, request.Status), cancellationToken);
        return ToActionResult(result);
    }
}

public record CancelOrderRequest(string Reason);
public record UpdateStatusRequest(OrderStatus Status);
