using ServerMonitoringAndNotificationSystem.Services;
using Microsoft.Extensions.Configuration;

namespace ServerMonitoringAndNotificationSystem;

class Program
{
    static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var interval = int.Parse(config["ServerStatisticsConfig:SamplingIntervalSeconds"]);

        var service = new StatCollectionService();

        while (true)
        {
            Console.WriteLine(service.GetServerStatistics().ToString());
            Thread.Sleep(interval*1000);
        }
    }
}