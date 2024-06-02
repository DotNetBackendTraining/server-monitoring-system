using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RabbitMQ.Client;
using ServerMonitoringSystem.Analyzer.Interfaces;
using ServerMonitoringSystem.Analyzer.Services;
using ServerMonitoringSystem.Common.Interfaces;
using ServerMonitoringSystem.Messaging.RabbitMq;
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
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
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

    public static void InjectRabbitMqConsumerServices(this IServiceCollection services)
    {
        services.InjectRabbitMqMessagingServices();
        services.AddTransient<IMessageConsumer>(p =>
        {
            var model = p.GetRequiredService<IModel>();
            var settings = new RabbitMqConsumerSettings
            {
                ExchangeName = "ServerStatisticsExchange",
                QueueName = "ServerStatisticsQueue",
                RoutingKey = "ServerStatistics.#"
            };
            return new RabbitMqMessageConsumer(settings, model);
        });
    }

    public static void InjectMongoDbPersistenceServices(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });

        services.AddSingleton<IPersistenceService, MongoDbPersistenceService>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            var mongoClient = sp.GetRequiredService<IMongoClient>();
            return new MongoDbPersistenceService(mongoClient, settings.ConnectionString, "ServerStatistics");
        });
    }
}