using System.Text.Json;
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
        Func<string, Task> onAlert,
        CancellationToken cancellationToken)
    {
        _messageConsumer.StartConsuming(async message =>
        {
            var statistics = Deserialize(message);
            await onSaveStatistics(statistics);

            if (_anomalyDetectorService.CheckForMemoryAnomaly(statistics))
            {
                await onAlert("Memory anomaly detected.");
            }

            if (_anomalyDetectorService.CheckForCpuAnomaly(statistics))
            {
                await onAlert("CPU anomaly detected.");
            }

            if (_anomalyDetectorService.CheckForHighMemoryUsage(statistics))
            {
                await onAlert("High memory usage detected.");
            }

            if (_anomalyDetectorService.CheckForHighCpuUsage(statistics))
            {
                await onAlert("High CPU usage detected.");
            }
        }, cancellationToken);
    }

    private ServerStatisticsData Deserialize(string message)
    {
        return JsonSerializer.Deserialize<ServerStatisticsData>(message)
               ?? throw new InvalidOperationException("Deserialization of server statistics failed.");
    }
}