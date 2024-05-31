namespace ServerMonitoringSystem.Collector.Interfaces;

/// <summary>
/// Performs tasks periodically like a clock, until its cancelled.
/// </summary>
public interface IClockService
{
    Task StartAsync(Func<Task> action, CancellationToken cancellationToken);
}