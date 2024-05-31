namespace ServerMonitoringSystem.Interfaces;

public interface IMessageProducer
{
    void SendMessage(string message);
}