using ServerMonitoringSystem.Collector.Models;

namespace ServerMonitoringSystem.Collector.Interfaces;

public interface IServerStatisticsGenerator
{
    Task<ServerStatisticsData> GenerateStatisticsAsync();
}