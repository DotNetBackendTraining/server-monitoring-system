namespace ServerMonitoringSystem.Messaging.RabbitMq.Configuration;

public class RabbitMqProducerSettings
{
    public string ExchangeName { get; set; }
    public string RoutingKey { get; set; }
}