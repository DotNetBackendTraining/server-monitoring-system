using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ServerMonitoringSystem.Messaging.RabbitMq.Configuration;

public static class ServicesConfigurationExtension
{
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
    }
}