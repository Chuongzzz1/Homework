using System;
using System.Threading;
using System.Threading.Tasks;

class ResourceIntensiveApp
{
    private const long ChunkSize = 512L * 1024 * 1024; // 512 MB
    private const long TotalMemory = 8L * 1024 * 1024 * 1024; // 8 GB

    static void Main()
    {
        int processorCount = Environment.ProcessorCount;

        Task[] cpuTasks = CreateCpuTasks(processorCount);
        Task memoryTask = AllocateMemory();

        Task.WaitAll(cpuTasks);
        memoryTask.Wait();
    }

    private static Task[] CreateCpuTasks(int processorCount)
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

    private static Task AllocateMemory()
    {
        return Task.Run(() =>
        {
            try
            {
                Console.WriteLine("Attempting to allocate 8GB of memory...");

                int numberOfChunks = (int)(TotalMemory / ChunkSize);
                var memoryChunks = new byte[numberOfChunks][];

                for (int i = 0; i < numberOfChunks; i++)
                {
                    memoryChunks[i] = new byte[ChunkSize];
                    InitializeMemory(memoryChunks[i]);
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

    private static void InitializeMemory(byte[] memoryChunk)
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