using ServerMonitoringSystem.Common.Models;

namespace ServerMonitoringSystem.Analyzer.Interfaces;

public interface IServerStatisticsProcessor
{
    public void StartProcessing(
        Func<ServerStatisticsData, Task> onSaveStatistics,
        Func<string, Task> onAlert,
        CancellationToken cancellationToken);
}