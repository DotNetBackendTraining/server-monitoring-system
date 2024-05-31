using System.Diagnostics;
using ServerMonitoringSystem.Collector.Interfaces;

namespace ServerMonitoringSystem.Collector.Services;

public class ClockService : IClockService
{
    private readonly TimeSpan _interval;

    public ClockService(TimeSpan interval)
    {
        _interval = interval;
    }

    public async Task StartAsync(Func<Task> action, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var stopwatch = Stopwatch.StartNew();
            await action();
            stopwatch.Stop();

            var elapsed = stopwatch.Elapsed;
            var delay = _interval - elapsed;

            // Take into account _action delay
            // Total delay should always equal _interval
            if (delay > TimeSpan.Zero)
            {
                await Task.Delay(delay, cancellationToken);
            }
        }
    }
}