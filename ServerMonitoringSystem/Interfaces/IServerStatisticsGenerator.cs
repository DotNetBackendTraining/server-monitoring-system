using ServerMonitoringSystem.Models;

namespace ServerMonitoringSystem.Interfaces;

public interface IServerStatisticsGenerator
{
    Task<ServerStatisticsData> GenerateStatisticsAsync();
}