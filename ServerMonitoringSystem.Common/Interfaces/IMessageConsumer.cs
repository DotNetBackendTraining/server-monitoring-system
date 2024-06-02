namespace ServerMonitoringSystem.Common.Interfaces;

public interface IMessageConsumer
{
    void StartConsuming(Func<string, Task> onMessage, CancellationToken cancellationToken);
}