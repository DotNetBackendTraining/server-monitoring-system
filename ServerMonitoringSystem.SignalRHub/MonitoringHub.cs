using Microsoft.AspNetCore.SignalR;

namespace ServerMonitoringSystem.SignalRHub;

public class MonitoringHub : Hub
{
    public async Task SendAlert(string message)
    {
        await Clients.All.SendAsync("ReceiveAlert", message);
    }
}