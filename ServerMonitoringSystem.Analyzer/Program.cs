using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServerMonitoringSystem.Analyzer.Common;
using ServerMonitoringSystem.Analyzer.Configuration;
using ServerMonitoringSystem.Analyzer.Interfaces;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();
services.InjectConfigurationServices(configuration);
services.InjectServerServices();
services.InjectSignalRAlerterServices();
services.InjectRabbitMqConsumerServices();
services.InjectMongoDbPersistenceServices();
var provider = services.BuildServiceProvider();

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

var alerter = provider.GetRequiredService<IAlertSender>();
var storage = provider.GetRequiredService<IPersistenceService>();
var processor = provider.GetRequiredService<IServerStatisticsProcessor>();

processor.StartProcessing(
    async statisticsData =>
    {
        Console.WriteLine(statisticsData);
        await storage.SaveServerStatisticsAsync(statisticsData);
    },
    async anomalyType =>
    {
        var message = $"One or more anomalies detected: {anomalyType.ToFriendlyString()}";
        Console.WriteLine(message);
        await alerter.SendAlertAsync(message);
    },
    cts.Token);

Console.WriteLine("Connection successful");
Console.WriteLine("Press [Enter] to exit.");
await Task.Delay(-1, cts.Token);

cts.Token.WaitHandle.WaitOne();
Console.WriteLine("Consumer stopped.");

provider.Dispose();