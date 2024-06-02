using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerMonitoringSystem.SignalREventConsumer.Configuration;
using ServerMonitoringSystem.SignalREventConsumer.Interfaces;
using ServerMonitoringSystem.SignalREventConsumer.Services;

var host = CreateHostBuilder(args).Build();
await host.RunAsync();

return;

IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((_, config) => { config.AddEnvironmentVariables(); })
        .ConfigureServices((context, services) =>
        {
            var configuration = context.Configuration;

            services.Configure<SignalRSettings>(configuration.GetSection("SignalRSettings"));

            services.AddSingleton<IEventHandler, ConsoleEventHandler>();
            services.AddHostedService<SignalREventConsumerService>();
        });