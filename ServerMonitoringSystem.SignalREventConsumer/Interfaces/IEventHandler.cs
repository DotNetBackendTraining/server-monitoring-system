namespace ServerMonitoringSystem.SignalREventConsumer.Interfaces;

public interface IEventHandler
{
    void HandleEvent(string message);
}