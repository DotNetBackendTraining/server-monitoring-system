using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServerMonitoringSystem.Common.Interfaces;
using ServerMonitoringSystem.Messaging.RabbitMq.Configuration;

namespace ServerMonitoringSystem.Messaging.RabbitMq;

public class RabbitMqMessageConsumer : IMessageConsumer, IDisposable
{
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;
    private readonly string _queueName;
    private string _consumerTag = string.Empty;

    public RabbitMqMessageConsumer(RabbitMqConsumerSettings settings, IModel channel)
    {
        var exchangeName = settings.ExchangeName;
        _queueName = settings.QueueName;
        var routingKey = settings.RoutingKey;
        _channel = channel;

        _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);
        _channel.QueueDeclare(queue: _queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        _channel.QueueBind(queue: _queueName, exchange: exchangeName, routingKey: routingKey);

        _consumer = new EventingBasicConsumer(_channel);
    }

    public void StartConsuming(Func<string, Task> onMessage, CancellationToken cancellationToken)
    {
        _consumer.Received += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            if (cancellationToken.IsCancellationRequested)
            {
                _channel.BasicCancel(_consumerTag);
                return;
            }

            await onMessage(message);
        };

        _consumerTag = _channel.BasicConsume(queue: _queueName,
            autoAck: true,
            consumer: _consumer);

        cancellationToken.Register(() => _channel.BasicCancel(_consumerTag));
    }

    public void Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.BasicCancel(_consumerTag);
            _channel.Close();
        }

        _consumer.Model.Dispose();
        GC.SuppressFinalize(this);
    }
}