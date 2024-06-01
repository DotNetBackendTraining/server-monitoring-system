namespace ServerMonitoringSystem.Analyzer.Interfaces;

public interface IAlertSender
{
    Task SendAlertAsync(string message);
}