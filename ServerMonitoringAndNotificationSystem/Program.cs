using ServerMonitoringAndNotificationSystem.Services;
using Microsoft.Extensions.Configuration;

namespace ServerMonitoringAndNotificationSystem;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var interval = int.Parse(config["ServerStatisticsConfig:SamplingIntervalSeconds"]);

        var service = new StatCollectionService();
        var producer = new ServerStatisticsProducer();

        while (true)
        {
            var stats = service.GetServerStatistics();
            await producer.Publish(stats);
            Thread.Sleep(interval*1000);
        }
    }
}