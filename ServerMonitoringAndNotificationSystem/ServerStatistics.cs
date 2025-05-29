namespace ServerMonitoringAndNotificationSystem;

public class ServerStatistics
{
    public double MemoryUsage { get; set; } // in MB
    public double AvailableMemory { get; set; } // in MB
    public double CpuUsage { get; set; }
    public DateTime Timestamp { get; set; }

    public override string ToString()
    {
        return $"[{Timestamp:yyyy-MM-dd HH:mm:ss}] " +
               $"CPU Usage: {CpuUsage:F2}%, " +
               $"Used Memory: {MemoryUsage:F2} MB, " +
               $"Available Memory: {AvailableMemory:F2} MB";
    }
}