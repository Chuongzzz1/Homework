using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class ResourceIntensiveApp
{
    public const int SLEEP_TIME = 1000;
    public const int MEMORY_CHUNK = 1;
    private const int BUSY_LOOP_MS = 10;
    private const double FULL_USAGE_CPU = 1.0;
    private const double USAGE_CPU = 0.8;
    public const long CHUNK_SIZE = 512L * 1024 * 1024; // 512 MB
    public const long TOTAL_MEMORY = 8L * 1024 * 1024 * 1024; // 8 GB

    public static void Main()
    {
        int processorCount = Environment.ProcessorCount;

        Task[] cpuTasks = CreateCpuTasks(processorCount, USAGE_CPU);
        Task memoryTask = AllocateMemory();

        Task.WaitAll(cpuTasks);
        memoryTask.Wait();
    }

    public static Task[] CreateCpuTasks(int processorCount, double targetCpuUsage)
    {
        var cpuTasks = new Task[processorCount];
        for (int i = 0; i < processorCount; i++)
        {
            cpuTasks[i] = Task.Run(() =>
            {
                var stopwatch = new System.Diagnostics.Stopwatch();
                double busyTime = BUSY_LOOP_MS * targetCpuUsage; 
                double idleTime = BUSY_LOOP_MS * (FULL_USAGE_CPU - targetCpuUsage);
                while (true)
                {
                    stopwatch.Restart();
                    while (stopwatch.ElapsedMilliseconds < busyTime) { }
                    Thread.Sleep((int)idleTime);
                }
            });
        }
        return cpuTasks;
    }

    public static Task AllocateMemory()
    {
        return Task.Run(() =>
        {
            try
            {
                Console.WriteLine("Attempting to allocate 8GB of memory...");

                int numberOfChunks = (int)(TOTAL_MEMORY / CHUNK_SIZE);
                var memoryChunks = new byte[numberOfChunks][];
                long totalAllocatedMemory = 0;

                for (int i = 0; i < numberOfChunks; i++)
                {
                    if (totalAllocatedMemory + CHUNK_SIZE > TOTAL_MEMORY)
                    {
                        Console.WriteLine("Reached the memory allocation limit of 8GB.");
                        break;
                    }

                    memoryChunks[i] = new byte[CHUNK_SIZE];
                    InitializeMemory(memoryChunks[i]);
                    totalAllocatedMemory += CHUNK_SIZE;
                }

                Console.WriteLine("Memory allocated successfully.");

                // Keep the task alive to retain the memory
                KeepMemoryAllocated();
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine("Out of memory!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        });
    }

    public static void InitializeMemory(byte[] memoryChunk)
    {
        for (int j = 0; j < memoryChunk.Length; j++)
        {
            memoryChunk[j] = MEMORY_CHUNK;
        }
    }

    private static void KeepMemoryAllocated()
    {
        while (true)
        {
            Thread.Sleep(SLEEP_TIME);
        }
    }
}