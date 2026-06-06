using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BubbleShop.Application.Common.Interfaces;
using BubbleShop.Infrastructure.Extensions;

namespace BubbleShop.Infrastructure.Services;

public class WhatsAppService : IWhatsAppService
{
    private readonly ILogger<WhatsAppService> _logger;
    private readonly WhatsAppOptions _options;

    public WhatsAppService(IOptions<WhatsAppOptions> options, ILogger<WhatsAppService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendTextMessageAsync(string toNumber, string message, CancellationToken cancellationToken = default)
    {
        // TODO: Replace stub with actual WhatsApp Business API (Meta Graph API) call
        _logger.LogInformation("[WhatsApp STUB] Sending to {Number}: {Message}", toNumber, message);
        await Task.CompletedTask;
    }

    public async Task SendTemplateMessageAsync(string toNumber, string templateName, object parameters, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("[WhatsApp STUB] Sending template '{Template}' to {Number}", templateName, toNumber);
        await Task.CompletedTask;
    }

    public Task<bool> ValidateWebhookSignatureAsync(string payload, string signature, CancellationToken cancellationToken = default)
    {
        // TODO: Implement HMAC-SHA256 signature validation using _options.AppSecret
        _logger.LogWarning("[WhatsApp STUB] Signature validation not yet implemented — returning true.");
        return Task.FromResult(true);
    }
}
