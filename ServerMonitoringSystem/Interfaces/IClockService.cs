namespace ServerMonitoringSystem.Interfaces;

/// <summary>
/// Performs a task periodically like a clock, until its cancelled.
/// </summary>
public interface IClockService
{
    Task StartAsync(CancellationToken cancellationToken);
}