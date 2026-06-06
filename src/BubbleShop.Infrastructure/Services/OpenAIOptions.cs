namespace BubbleShop.Infrastructure.Services;

public class OpenAIOptions
{
    public const string SectionName = "OpenAI";

    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = "gpt-4o";
    public string? AzureEndpoint { get; set; }
    public string? AzureDeploymentName { get; set; }
    public int MaxTokens { get; set; } = 1024;
    public double Temperature { get; set; } = 0.7;
}
