using Microsoft.AspNetCore.Mvc;
using BubbleShop.Application.Features.WhatsApp.Commands.HandleIncomingMessage;

namespace BubbleShop.API.Controllers;

public class WhatsAppController : ApiControllerBase
{
    /// <summary>
    /// WhatsApp webhook verification (GET) — responds to Meta's hub.challenge.
    /// </summary>
    [HttpGet("webhook")]
    public IActionResult Verify(
        [FromQuery(Name = "hub.mode")] string mode,
        [FromQuery(Name = "hub.verify_token")] string verifyToken,
        [FromQuery(Name = "hub.challenge")] string challenge,
        [FromServices] IConfiguration configuration)
    {
        var expectedToken = configuration["WhatsApp:VerifyToken"];
        if (mode == "subscribe" && verifyToken == expectedToken)
            return Ok(challenge);

        return Forbid();
    }

    /// <summary>
    /// WhatsApp webhook events (POST) — handles inbound messages from Meta.
    /// </summary>
    [HttpPost("webhook")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Receive([FromBody] WhatsAppWebhookPayload payload, CancellationToken cancellationToken)
    {
        foreach (var entry in payload.Entry ?? [])
        {
            foreach (var change in entry.Changes ?? [])
            {
                var messages = change.Value?.Messages;
                if (messages is null) continue;

                foreach (var message in messages)
                {
                    if (message.Type != "text") continue;

                    await Mediator.Send(new HandleIncomingMessageCommand(
                        message.From,
                        message.Text?.Body ?? string.Empty,
                        message.Id), cancellationToken);
                }
            }
        }

        return Ok();
    }
}

// Minimal binding classes for Meta's webhook structure
public record WhatsAppWebhookPayload(string? Object, List<WhatsAppEntry>? Entry);
public record WhatsAppEntry(string? Id, List<WhatsAppChange>? Changes);
public record WhatsAppChange(string? Field, WhatsAppChangeValue? Value);
public record WhatsAppChangeValue(List<WhatsAppMessage>? Messages, List<object>? Statuses);
public record WhatsAppMessage(string Id, string From, string Type, WhatsAppText? Text);
public record WhatsAppText(string Body);
