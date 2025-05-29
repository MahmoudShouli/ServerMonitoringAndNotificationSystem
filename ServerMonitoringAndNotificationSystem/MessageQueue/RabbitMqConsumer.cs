using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ServerMonitoringAndNotificationSystem.MessageQueue;

public class RabbitMqConsumer: IStatsConsumer
{
    private const string QueueName = "server-stats";
    private const string ExchangeName = "server-stats-exchange";

    public async Task Consume()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        
        // Ensure the setup is made (idempotent)
        await channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic);
        await channel.QueueDeclareAsync(QueueName,false,false,false,null);
        await channel.QueueBindAsync(QueueName, ExchangeName, "ServerStatistics.*");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            try
            {
                var stats = JsonSerializer.Deserialize<ServerStatistics>(json);
                Console.WriteLine($"Consumer received: {stats}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error parsing message: {ex.Message}");
            }

            await Task.Yield(); // allow async handler
        };

        await channel.BasicConsumeAsync(queue: QueueName, autoAck: true, consumer: consumer);
    }
}