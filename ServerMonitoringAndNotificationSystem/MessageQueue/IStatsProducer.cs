namespace ServerMonitoringAndNotificationSystem.MessageQueue;

public interface IStatsProducer
{
    Task Publish(ServerStatistics stats);
}