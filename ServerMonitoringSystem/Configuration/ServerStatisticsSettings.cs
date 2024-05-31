namespace ServerMonitoringSystem.Configuration;

public class ServerStatisticsSettings
{
    public int CpuUsageSamplingMilliSeconds { get; set; }
    public double SamplingIntervalSeconds { get; set; }
    public string ServerIdentifier { get; set; }
}