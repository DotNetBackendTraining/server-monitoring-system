using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServerMonitoringSystem.Analyzer.Configuration;
using ServerMonitoringSystem.Analyzer.Interfaces;
using ServerMonitoringSystem.Common.Interfaces;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();
services.InjectConfigurationServices(configuration);
services.InjectSignalRAlerterServices();
services.InjectRabbitMqConsumerServices();
var provider = services.BuildServiceProvider();

var alerter = provider.GetRequiredService<IAlertSender>();
await alerter.SendAlertAsync("Example alert");

var consumer = provider.GetRequiredService<IMessageConsumer>();
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

consumer.StartConsuming(async message =>
{
    Console.WriteLine(message);
    await Task.CompletedTask;
}, cts.Token);

Console.WriteLine("Connection successful");
Console.WriteLine("Press [Enter] to exit.");
await Task.Delay(-1, cts.Token);

cts.Token.WaitHandle.WaitOne();
Console.WriteLine("Consumer stopped.");

provider.Dispose();