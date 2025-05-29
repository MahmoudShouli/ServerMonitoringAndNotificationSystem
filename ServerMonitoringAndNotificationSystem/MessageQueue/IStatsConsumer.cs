namespace ServerMonitoringAndNotificationSystem.MessageQueue;

public interface IStatsConsumer
{
    Task Consume();
}