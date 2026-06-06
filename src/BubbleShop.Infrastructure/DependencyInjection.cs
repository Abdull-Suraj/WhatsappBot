using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BubbleShop.Application.Common.Interfaces;
using BubbleShop.Infrastructure.Extensions;
using BubbleShop.Infrastructure.Persistence;
using BubbleShop.Infrastructure.Services;

namespace BubbleShop.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.Configure<WhatsAppOptions>(configuration.GetSection(WhatsAppOptions.SectionName));
        services.AddScoped<IWhatsAppService, WhatsAppService>();

        services.Configure<StripeOptions>(configuration.GetSection(StripeOptions.SectionName));
        services.AddScoped<IPaymentService, StripePaymentService>();

        services.AddScoped<IDeliveryService, DeliveryService>();

        services.Configure<OpenAIOptions>(configuration.GetSection(OpenAIOptions.SectionName));
        services.AddScoped<IAIAgentService, OpenAIAgentService>();

        return services;
    }
}
