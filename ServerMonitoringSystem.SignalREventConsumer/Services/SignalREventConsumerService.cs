using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ServerMonitoringSystem.SignalREventConsumer.Configuration;
using ServerMonitoringSystem.SignalREventConsumer.Interfaces;

namespace ServerMonitoringSystem.SignalREventConsumer.Services;

public class SignalREventConsumerService : IHostedService
{
    private readonly HubConnection _connection;
    private readonly IEventHandler _eventHandler;

    public SignalREventConsumerService(IOptions<SignalRSettings> settings, IEventHandler eventHandler)
    {
        _eventHandler = eventHandler;
        var signalRSettings = settings.Value;
        _connection = new HubConnectionBuilder()
            .WithUrl(signalRSettings.SignalRUrl)
            .WithAutomaticReconnect()
            .Build();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _connection.On<string>("ReceiveMessage", message => _eventHandler.HandleEvent(message));

        await _connection.StartAsync(cancellationToken);
        Console.WriteLine("SignalR connection started.");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _connection.StopAsync(cancellationToken);
        Console.WriteLine("SignalR connection stopped.");
    }
}