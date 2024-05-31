using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServerMonitoringSystem.Configuration;
using ServerMonitoringSystem.Interfaces;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();
services.InjectServerServices(configuration.GetSection("ServerStatisticsSettings"));
var provider = services.BuildServiceProvider();

var generator = provider.GetRequiredService<IServerStatisticsGenerator>();
var clock = provider.GetRequiredService<IClockService>();

await clock.StartAsync(
    async () => Console.WriteLine(await generator.GenerateStatisticsAsync()),
    CancellationToken.None);