namespace ServerMonitoringSystem.Collector.Models;

public record ServerStatisticsData(
    double MemoryUsage,
    double AvailableMemory,
    double CpuUsage,
    DateTime Timestamp)
{
}