namespace ServerMonitoringSystem.Common.Interfaces;

public interface IMessageProducer
{
    void SendMessage(string message);
}