using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ServerMonitoringSystem.Analyzer.Interfaces;
using ServerMonitoringSystem.Analyzer.Services;
using ServerMonitoringSystem.Messaging.RabbitMq.Configuration;

namespace ServerMonitoringSystem.Analyzer.Configuration;

public static class ServicesConfigurationExtension
{
    public static void InjectConfigurationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AnomalyDetectionSettings>(configuration.GetSection("AnomalyDetectionSettings"));
        services.Configure<SignalRSettings>(configuration.GetSection("SignalRSettings"));
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));
    }

    public static void InjectSignalRAlerterServices(this IServiceCollection services)
    {
        services.AddSingleton<HubConnection>(p =>
        {
            var settings = p.GetRequiredService<IOptions<SignalRSettings>>().Value;
            var hubConnection = new HubConnectionBuilder()
                .WithUrl(settings.SignalRUrl)
                .WithAutomaticReconnect()
                .Build();

            hubConnection.StartAsync().Wait();
            return hubConnection;
        });
        services.AddSingleton<IAlertSender, SignalRAlertSender>();
    }
}