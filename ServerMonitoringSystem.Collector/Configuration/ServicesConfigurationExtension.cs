using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using ServerMonitoringSystem.Collector.Interfaces;
using ServerMonitoringSystem.Collector.Services;

namespace ServerMonitoringSystem.Collector.Configuration;

public static class ServicesConfigurationExtension
{
    public static void InjectConfigurationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<ServerStatisticsSettings>(configuration.GetSection("ServerStatisticsSettings"));
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));
    }

    public static void InjectServerServices(this IServiceCollection services)
    {
        services.AddScoped<IServerStatisticsGenerator, ServerStatisticsGenerator>(p =>
        {
            var settings = p.GetRequiredService<IOptions<ServerStatisticsSettings>>().Value;
            return new ServerStatisticsGenerator(settings.CpuUsageSamplingMilliSeconds);
        });
        services.AddScoped<IClockService>(p =>
        {
            var settings = p.GetRequiredService<IOptions<ServerStatisticsSettings>>().Value;
            return new ClockService(TimeSpan.FromSeconds(settings.SamplingIntervalSeconds));
        });
    }

    public static void InjectRabbitMqMessagingServices(this IServiceCollection services)
    {
        services.AddSingleton<IConnection>(p =>
        {
            var settings = p.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
            var factory = new ConnectionFactory
            {
                HostName = settings.Hostname,
                UserName = settings.Username,
                Password = settings.Password
            };
            return factory.CreateConnection();
        });

        services.AddTransient<IModel>(p => p.GetRequiredService<IConnection>().CreateModel());
        services.AddTransient<IMessageProducer>(p =>
        {
            var settings = p.GetRequiredService<IOptions<ServerStatisticsSettings>>().Value;
            var model = p.GetRequiredService<IModel>();
            return new RabbitMqMessageProducer("ServerStatistics." + settings.ServerIdentifier, model);
        });
    }
}