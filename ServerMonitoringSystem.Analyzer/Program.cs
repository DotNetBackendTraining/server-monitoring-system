using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServerMonitoringSystem.Analyzer.Configuration;
using ServerMonitoringSystem.Analyzer.Interfaces;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();
services.InjectConfigurationServices(configuration);
services.InjectSignalRAlerterServices();
var provider = services.BuildServiceProvider();

var alerter = provider.GetRequiredService<IAlertSender>();
await alerter.SendAlertAsync("Example alert");