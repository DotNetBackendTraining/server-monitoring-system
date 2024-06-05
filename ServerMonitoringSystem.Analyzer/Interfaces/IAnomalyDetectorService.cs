using ServerMonitoringSystem.Analyzer.Common;
using ServerMonitoringSystem.Common.Models;

namespace ServerMonitoringSystem.Analyzer.Interfaces;

public interface IAnomalyDetectorService
{
    AnomalyType CheckForAnomaly(ServerStatisticsData current);
}