namespace ServerMonitoringSystem.Collector.Interfaces;

public interface IMessageProducer
{
    void SendMessage(string message);
}