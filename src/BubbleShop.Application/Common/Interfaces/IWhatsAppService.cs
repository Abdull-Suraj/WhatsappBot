namespace BubbleShop.Application.Common.Interfaces;

public interface IWhatsAppService
{
    Task SendTextMessageAsync(string toNumber, string message, CancellationToken cancellationToken = default);
    Task SendTemplateMessageAsync(string toNumber, string templateName, object parameters, CancellationToken cancellationToken = default);
    Task<bool> ValidateWebhookSignatureAsync(string payload, string signature, CancellationToken cancellationToken = default);
}
