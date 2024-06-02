using ServerMonitoringSystem.Analyzer.Configuration;
using ServerMonitoringSystem.Analyzer.Interfaces;
using ServerMonitoringSystem.Common.Models;

namespace ServerMonitoringSystem.Analyzer.Services;

public class AnomalyDetectorService : IAnomalyDetectorService
{
    private readonly AnomalyDetectionSettings _settings;
    private ServerStatisticsData? _previousStatistics;

    public AnomalyDetectorService(AnomalyDetectionSettings settings)
    {
        _settings = settings;
    }

    public bool CheckForMemoryAnomaly(ServerStatisticsData current)
    {
        if (_previousStatistics == null) return false;

        var isAnomaly = current.MemoryUsage >
                        _previousStatistics.MemoryUsage * (1 + _settings.MemoryUsageAnomalyThresholdPercentage);

        _previousStatistics = current;
        return isAnomaly;
    }

    public bool CheckForCpuAnomaly(ServerStatisticsData current)
    {
        if (_previousStatistics == null) return false;

        var isAnomaly = current.CpuUsage >
                        _previousStatistics.CpuUsage * (1 + _settings.CpuUsageAnomalyThresholdPercentage);

        _previousStatistics = current;
        return isAnomaly;
    }

    public bool CheckForHighMemoryUsage(ServerStatisticsData current)
    {
        if (current is { MemoryUsage: 0, AvailableMemory: 0 }) return false;

        var isHighUsage = current.MemoryUsage / (current.MemoryUsage + current.AvailableMemory) >
                          _settings.MemoryUsageThresholdPercentage;
        return isHighUsage;
    }

    public bool CheckForHighCpuUsage(ServerStatisticsData current)
    {
        var isHighUsage = current.CpuUsage > _settings.CpuUsageThresholdPercentage;
        return isHighUsage;
    }
}