using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServerMonitoringSystem.Collector.Configuration;
using ServerMonitoringSystem.Collector.Interfaces;
using ServerMonitoringSystem.Common.Interfaces;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();
services.InjectConfigurationServices(configuration);
services.InjectServerServices();
services.InjectRabbitMqProducerServices();
var provider = services.BuildServiceProvider();

var generator = provider.GetRequiredService<IServerStatisticsGenerator>();
var clock = provider.GetRequiredService<IClockService>();
var producer = provider.GetRequiredService<IMessageProducer>();

await clock.StartAsync(
    async () =>
    {
        var message = JsonSerializer.Serialize(await generator.GenerateStatisticsAsync());
        Console.WriteLine(message);
        producer.SendMessage(message);
    },
    CancellationToken.None);