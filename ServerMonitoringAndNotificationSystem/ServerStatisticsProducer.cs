using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace ServerMonitoringAndNotificationSystem;

public class ServerStatisticsProducer
{
    private const string QueueName = "server-stats";
    private const string ExchangeName = "server-stats-exchange";

    public async Task Publish(ServerStatistics stats)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        
        var json = JsonSerializer.Serialize(stats);
        var body = Encoding.UTF8.GetBytes(json);
        
        // Declare the exchange component and the message queue
        await channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic);
        await channel.QueueDeclareAsync(QueueName,false,false,false,null);
        // Bind the queue to the exchange
        await channel.QueueBindAsync(QueueName, ExchangeName, "ServerStatistics.*");
        await channel.BasicPublishAsync(
            exchange: ExchangeName,
            routingKey: "ServerStatistics." + GetServerIdentifier(),
            body: body
        );

        Console.WriteLine($"Published: {stats}");
    }

    private string GetServerIdentifier()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        return config["ServerStatisticsConfig:ServerIdentifier"] ?? string.Empty;
    }
}