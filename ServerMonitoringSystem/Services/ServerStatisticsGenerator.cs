using System.Diagnostics;
using ServerMonitoringSystem.Interfaces;
using ServerMonitoringSystem.Models;

namespace ServerMonitoringSystem.Services;

public class ServerStatisticsGenerator : IServerStatisticsGenerator
{
    private readonly int _cpuUsageSamplingDelay;

    public ServerStatisticsGenerator(int cpuUsageSamplingDelay)
    {
        _cpuUsageSamplingDelay = cpuUsageSamplingDelay;
    }

    public async Task<ServerStatisticsData> GenerateStatisticsAsync()
    {
        var availableMemory = GetAvailableMemory();
        var totalMemory = GetTotalPhysicalMemory();
        var memoryUsage = ((totalMemory - availableMemory) / totalMemory) * 100;
        var cpuUsage = await GetCpuUsageAsync();

        return new ServerStatisticsData(
            memoryUsage,
            availableMemory,
            cpuUsage,
            DateTime.Now);
    }

    private static double GetAvailableMemory()
    {
        var info = GC.GetGCMemoryInfo();
        return (info.TotalAvailableMemoryBytes - info.TotalCommittedBytes) / (1024.0 * 1024.0);
    }

    private static double GetTotalPhysicalMemory()
    {
        return GC.GetGCMemoryInfo().TotalAvailableMemoryBytes / (1024.0 * 1024.0);
    }

    private async Task<double> GetCpuUsageAsync()
    {
        var startTime = DateTime.UtcNow;
        var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
        await Task.Delay(_cpuUsageSamplingDelay);
        var endTime = DateTime.UtcNow;
        var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;

        var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
        var totalMsPassed = (endTime - startTime).TotalMilliseconds;

        return (cpuUsedMs / (Environment.ProcessorCount * totalMsPassed)) * 100;
    }
}