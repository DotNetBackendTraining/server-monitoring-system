using ServerMonitoringSystem.Common.Models;

namespace ServerMonitoringSystem.Collector.Interfaces;

public interface IServerStatisticsGenerator
{
    Task<ServerStatisticsData> GenerateStatisticsAsync();
}