namespace ServerMonitoringSystem.Analyzer.Common;

[Flags]
public enum AnomalyType
{
    None = 0,
    Memory = 1 << 0,
    Cpu = 1 << 1,
    HighMemoryUsage = 1 << 2,
    HighCpuUsage = 1 << 3
}