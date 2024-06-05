using ServerMonitoringSystem.Analyzer.Common;
using ServerMonitoringSystem.Common.Models;

namespace ServerMonitoringSystem.Analyzer.Interfaces;

public interface IServerStatisticsProcessor
{
    public void StartProcessing(
        Func<ServerStatisticsData, Task> onSaveStatistics,
        Func<AnomalyType, Task> onAlert,
        CancellationToken cancellationToken);
}