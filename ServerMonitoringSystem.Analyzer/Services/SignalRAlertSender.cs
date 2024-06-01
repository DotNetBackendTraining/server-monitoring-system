using Microsoft.AspNetCore.SignalR.Client;
using ServerMonitoringSystem.Analyzer.Interfaces;

namespace ServerMonitoringSystem.Analyzer.Services;

public class SignalRAlertSender : IAlertSender
{
    private readonly HubConnection _connection;

    public SignalRAlertSender(HubConnection connection)
    {
        _connection = connection;
    }

    public async Task SendAlertAsync(string message)
    {
        await _connection.InvokeAsync("SendAlert", message);
    }
}