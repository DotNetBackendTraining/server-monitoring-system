using ServerMonitoringSystem.Common.Models;

namespace ServerMonitoringSystem.Analyzer.Interfaces;

public interface IAnomalyDetectorService
{
    bool CheckForMemoryAnomaly(ServerStatisticsData current);

    bool CheckForCpuAnomaly(ServerStatisticsData current);

    bool CheckForHighMemoryUsage(ServerStatisticsData current);

    bool CheckForHighCpuUsage(ServerStatisticsData current);
}