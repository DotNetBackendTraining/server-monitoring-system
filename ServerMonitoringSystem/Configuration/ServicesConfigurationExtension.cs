using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServerMonitoringSystem.Interfaces;
using ServerMonitoringSystem.Services;

namespace ServerMonitoringSystem.Configuration;

public static class ServicesConfigurationExtension
{
    public static void InjectServerServices(
        this IServiceCollection services,
        IConfigurationSection serverStatisticsConfiguration)
    {
        var cpuUsageSamplingMilliSeconds = int.Parse(serverStatisticsConfiguration["CpuUsageSamplingMilliSeconds"]!);
        var samplingIntervalSeconds = double.Parse(serverStatisticsConfiguration["SamplingIntervalSeconds"]!);

        services.AddScoped<IServerStatisticsGenerator, ServerStatisticsGenerator>(_ =>
            new ServerStatisticsGenerator(cpuUsageSamplingMilliSeconds));
        services.AddScoped<IClockService>(_ =>
            new ClockService(TimeSpan.FromSeconds(samplingIntervalSeconds)));
    }
}