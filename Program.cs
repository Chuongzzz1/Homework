using System;
using System.Threading;
using System.Threading.Tasks;

public class ResourceIntensiveApp
{
    public const long CHUNK_SIZE = 512L * 1024 * 1024; // 512 MB
    public const long TOTAL_MEMORY = 8L * 1024 * 1024 * 1024; // 8 GB

    public static void Main()
    {
        int processorCount = Environment.ProcessorCount;

        Task[] cpuTasks = CreateCpuTasks(processorCount);
        Task memoryTask = AllocateMemory();

        Task.WaitAll(cpuTasks);
        memoryTask.Wait();
    }

    public static Task[] CreateCpuTasks(int processorCount)
    {
        var cpuTasks = new Task[processorCount];
        for (int i = 0; i < processorCount; i++)
        {
            cpuTasks[i] = Task.Run(() =>
            {
                while (true)
                {
                    // Busy loop to consume CPU
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
            memoryChunk[j] = 1;
        }
    }

    private static void KeepMemoryAllocated()
    {
        while (true)
        {
            Thread.Sleep(1000);
        }
    }
}