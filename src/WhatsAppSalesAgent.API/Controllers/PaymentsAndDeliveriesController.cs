using Microsoft.AspNetCore.Mvc;
using WhatsAppSalesAgent.Application.Features.Deliveries.Commands.ArrangeDelivery;
using WhatsAppSalesAgent.Application.Features.Deliveries.Queries.GetDeliveryStatus;
using WhatsAppSalesAgent.Application.Features.Payments.Commands.HandlePaymentWebhook;
using WhatsAppSalesAgent.Application.Features.Payments.Commands.InitiatePayment;

namespace WhatsAppSalesAgent.API.Controllers;

public class PaymentsController : ApiControllerBase
{
    [HttpPost("{orderId:guid}/initiate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Initiate(Guid orderId, [FromQuery] string currency = "GBP", CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new InitiatePaymentCommand(orderId, currency), cancellationToken);
        return ToActionResult(result);
    }

    [HttpPost("webhook")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Webhook(CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(Request.Body);
        var payload = await reader.ReadToEndAsync(cancellationToken);
        var signature = Request.Headers["Stripe-Signature"].FirstOrDefault() ?? string.Empty;

        var result = await Mediator.Send(new HandlePaymentWebhookCommand(payload, signature), cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest();
    }
}

public class DeliveriesController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Arrange([FromBody] ArrangeDeliveryCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("{trackingNumber}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatus(string trackingNumber, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetDeliveryStatusQuery(trackingNumber), cancellationToken);
        return ToActionResult(result);
    }
}
