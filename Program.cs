using System;
using System.Threading;
using System.Threading.Tasks;

class ResourceIntensiveApp
{
    static void Main()
    {
        int processorCount = Environment.ProcessorCount;

        // Start threads to consume CPU
        Task[] cpuTasks = new Task[processorCount];
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

        // Start a thread to consume a large amount of memory
        Task memoryTask = Task.Run(() =>
        {
            try
            {
                Console.WriteLine("Attempting to allocate 8GB of memory...");

                // Divide the memory allocation into smaller chunks
                const long chunkSize = 1024L * 1024 * 512; // 512 MB
                long totalMemory = 8L * 1024 * 1024 * 1024; // 8 GB
                int numberOfChunks = (int)(totalMemory / chunkSize);
                byte[][] memoryChunks = new byte[numberOfChunks][];

                for (int i = 0; i < numberOfChunks; i++)
                {
                    memoryChunks[i] = new byte[chunkSize];
                    // Initialize the array to ensure the memory is allocated
                    for (long j = 0; j < chunkSize; j++)
                    {
                        memoryChunks[i][j] = 1;
                    }
                }

                Console.WriteLine("Memory allocated successfully.");

                // Keep the task alive to retain the memory
                while (true)
                {
                    Thread.Sleep(1000);
                }
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine("Out of memory!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        });

        // Wait for all CPU tasks to complete (they never will)
        Task.WaitAll(cpuTasks);

        // Wait for the memory task (it never will)
        memoryTask.Wait();
    }
}
