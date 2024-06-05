using System.Text.Json;
using ServerMonitoringSystem.Analyzer.Common;
using ServerMonitoringSystem.Analyzer.Interfaces;
using ServerMonitoringSystem.Common.Interfaces;
using ServerMonitoringSystem.Common.Models;

namespace ServerMonitoringSystem.Analyzer.Services;

public class ServerStatisticsProcessor : IServerStatisticsProcessor
{
    private readonly IMessageConsumer _messageConsumer;
    private readonly IAnomalyDetectorService _anomalyDetectorService;

    public ServerStatisticsProcessor(
        IMessageConsumer messageConsumer,
        IAnomalyDetectorService anomalyDetectorService)
    {
        _messageConsumer = messageConsumer;
        _anomalyDetectorService = anomalyDetectorService;
    }

    public void StartProcessing(
        Func<ServerStatisticsData, Task> onSaveStatistics,
        Func<AnomalyType, Task> onAlert,
        CancellationToken cancellationToken)
    {
        _messageConsumer.StartConsuming(async message =>
        {
            var statistics = Deserialize(message);
            await onSaveStatistics(statistics);

            var anomaly = _anomalyDetectorService.CheckForAnomaly(statistics);
            if (anomaly != AnomalyType.None)
            {
                await onAlert(anomaly);
            }
        }, cancellationToken);
    }

    private ServerStatisticsData Deserialize(string message)
    {
        return JsonSerializer.Deserialize<ServerStatisticsData>(message)
               ?? throw new InvalidOperationException("Deserialization of server statistics failed.");
    }
}