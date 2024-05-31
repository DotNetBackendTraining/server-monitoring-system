using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ServerMonitoringSystem.Interfaces;
using ServerMonitoringSystem.Services;

namespace ServerMonitoringSystem.Configuration;

public static class ServicesConfigurationExtension
{
    public static void InjectConfigurationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<ServerStatisticsSettings>(configuration.GetSection("ServerStatisticsSettings"));
    }

    public static void InjectServerServices(this IServiceCollection services)
    {
        services.AddScoped<IServerStatisticsGenerator, ServerStatisticsGenerator>(p =>
        {
            var settings = p.GetRequiredService<IOptions<ServerStatisticsSettings>>().Value;
            return new ServerStatisticsGenerator(settings.CpuUsageSamplingMilliSeconds);
        });
        services.AddScoped<IClockService>(p =>
        {
            var settings = p.GetRequiredService<IOptions<ServerStatisticsSettings>>().Value;
            return new ClockService(TimeSpan.FromSeconds(settings.SamplingIntervalSeconds));
        });
    }

        services.AddScoped<IServerStatisticsGenerator, ServerStatisticsGenerator>(_ =>
            new ServerStatisticsGenerator(cpuUsageSamplingMilliSeconds));
        services.AddScoped<IClockService>(_ =>
            new ClockService(TimeSpan.FromSeconds(samplingIntervalSeconds)));
    }
}