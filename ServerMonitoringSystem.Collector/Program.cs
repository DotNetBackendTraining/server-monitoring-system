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
var cts = new CancellationTokenSource();

AppDomain.CurrentDomain.ProcessExit += (s, e) =>
{
    Console.WriteLine("Process exiting...");
    cts.Cancel();
};

Console.CancelKeyPress += (_, e) =>
{
    Console.WriteLine("Canceling...");
    cts.Cancel();
    e.Cancel = true;
};

await clock.StartAsync(
    async () =>
    {
        var message = JsonSerializer.Serialize(await generator.GenerateStatisticsAsync());
        Console.WriteLine(message);
        producer.SendMessage(message);
    },
    cts.Token);

Console.WriteLine("Press [Enter] to exit.");
Console.ReadLine();

cts.Cancel();
provider.Dispose();
Console.WriteLine("Application stopped.");