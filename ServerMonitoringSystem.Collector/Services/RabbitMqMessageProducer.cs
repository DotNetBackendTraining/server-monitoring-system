using System.Text;
using RabbitMQ.Client;
using ServerMonitoringSystem.Common.Interfaces;

namespace ServerMonitoringSystem.Collector.Services;

public class RabbitMqMessageProducer : IMessageProducer, IDisposable
{
    private readonly string _queueName;
    private readonly IModel _channel;

    public RabbitMqMessageProducer(string queueName, IModel channel)
    {
        _queueName = queueName;
        _channel = channel;

        _channel.QueueDeclare(queue: _queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    public void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "",
            routingKey: _queueName,
            basicProperties: null,
            body: body);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _channel.Close();
    }
}