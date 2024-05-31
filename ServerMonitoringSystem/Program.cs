using Microsoft.Extensions.Configuration;
using ServerMonitoringSystem.Services;

var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables();

var configuration = builder.Build();
var serverStatisticsConfig = configuration.GetSection("ServerStatisticsSettings");

var samplingIntervalSeconds = double.Parse(
    serverStatisticsConfig["SamplingIntervalSeconds"]
    ?? throw new IOException("SamplingIntervalSeconds not defined"));

var cpuUsageSamplingMilliSeconds = int.Parse(
    serverStatisticsConfig["CpuUsageSamplingMilliSeconds"]
    ?? throw new IOException("CpuUsageSamplingMilliSeconds not defined"));

var serverIdentifier =
    serverStatisticsConfig["ServerIdentifier"]
    ?? throw new IOException("ServerIdentifier not defined");

Console.WriteLine($"Sampling Interval: {samplingIntervalSeconds} seconds");
Console.WriteLine($"Server Identifier: {serverIdentifier}");
var generator = new ServerStatisticsGenerator(settings.CpuUsageSamplingMilliSeconds);
var clock = new ClockService(TimeSpan.FromSeconds(settings.SamplingIntervalSeconds));
await clock.StartAsync(
    async () => Console.WriteLine(await generator.GenerateStatisticsAsync()),
    CancellationToken.None);