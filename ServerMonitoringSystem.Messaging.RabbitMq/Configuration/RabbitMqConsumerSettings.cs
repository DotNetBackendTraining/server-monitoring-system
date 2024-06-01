namespace ServerMonitoringSystem.Messaging.RabbitMq.Configuration;

public class RabbitMqConsumerSettings
{
    public string ExchangeName { get; set; }
    public string QueueName { get; set; }
    public string RoutingKey { get; set; }
}