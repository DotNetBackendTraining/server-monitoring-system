using ServerMonitoringSystem.Common.Models;

namespace ServerMonitoringSystem.Analyzer.Interfaces;

public interface IPersistenceService
{
    Task SaveServerStatisticsAsync(ServerStatisticsData data);
}