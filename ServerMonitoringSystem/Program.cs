using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables();

var configuration = builder.Build();
var serverStatisticsConfig = configuration.GetSection("ServerStatisticsSettings");

var samplingIntervalSeconds = serverStatisticsConfig["SamplingIntervalSeconds"];
var serverIdentifier = serverStatisticsConfig["ServerIdentifier"];

Console.WriteLine($"Sampling Interval: {samplingIntervalSeconds} seconds");
Console.WriteLine($"Server Identifier: {serverIdentifier}");