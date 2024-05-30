using System.Diagnostics;
using ServerMonitoringSystem.Interfaces;

namespace ServerMonitoringSystem.Services;

public class ClockService : IClockService
{
    private readonly TimeSpan _interval;
    private readonly Func<Task> _action;

    public ClockService(TimeSpan interval, Func<Task> action)
    {
        _interval = interval;
        _action = action;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var stopwatch = Stopwatch.StartNew();
            await _action();
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