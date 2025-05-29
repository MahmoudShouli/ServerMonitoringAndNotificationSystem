using System.Diagnostics;
using System.Management;

namespace ServerMonitoringAndNotificationSystem.Services;

public class StatCollectionService
{
    public ServerStatistics GetServerStatistics()
    {
        var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        var memCounter = new PerformanceCounter("Memory", "Available MBytes");
        
        _ = cpuCounter.NextValue();
        Thread.Sleep(1000);
        
        double cpuUsage = cpuCounter.NextValue();
        double availableMb = memCounter.NextValue();
        double totalMb = GetTotalMemoryInMb();
        double usedMb = totalMb - availableMb;
        
        ServerStatistics serverStatistics = new()
        {
            MemoryUsage = usedMb,
            AvailableMemory = availableMb,
            CpuUsage = cpuUsage,
            Timestamp = DateTime.Now
        };
        
        return serverStatistics;
    }

    private double GetTotalMemoryInMb()
    {
        double totalMemory = 0;

        var searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
        foreach (ManagementObject obj in searcher.Get())
        {
            totalMemory = Convert.ToSingle(obj["TotalPhysicalMemory"]) / (1024 * 1024);
        }

        return totalMemory;
    }
}