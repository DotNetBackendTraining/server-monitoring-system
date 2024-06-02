using ServerMonitoringSystem.SignalREventConsumer.Interfaces;

namespace ServerMonitoringSystem.SignalREventConsumer.Services;

public class ConsoleEventHandler : IEventHandler
{
    public void HandleEvent(string message)
    {
        Console.WriteLine($"Received message: {message}");
    }
}