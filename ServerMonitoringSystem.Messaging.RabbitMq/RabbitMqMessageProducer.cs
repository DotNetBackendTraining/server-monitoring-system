using System.Text;
using RabbitMQ.Client;
using ServerMonitoringSystem.Common.Interfaces;
using ServerMonitoringSystem.Messaging.RabbitMq.Configuration;

namespace ServerMonitoringSystem.Messaging.RabbitMq;

public class RabbitMqMessageProducer : IMessageProducer, IDisposable
{
    private readonly IModel _channel;
    private readonly string _exchangeName;
    private readonly string _routingKey;

    public RabbitMqMessageProducer(RabbitMqProducerSettings settings, IModel channel)
    {
        _exchangeName = settings.ExchangeName;
        _routingKey = settings.RoutingKey;
        _channel = channel;

        _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Topic);
    }

    public void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: _exchangeName,
            routingKey: _routingKey,
            basicProperties: null,
            body: body);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _channel.Close();
    }
}