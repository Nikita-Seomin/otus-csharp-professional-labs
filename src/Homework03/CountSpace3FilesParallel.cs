using System.Collections.Concurrent;

namespace Homework03;

public static class CounterSpace
{
    public static async Task<int> Count(FileInfo file)
    {
        int spaceCount = 0;
        using (FileStream fs = File.OpenRead(file.FullName))
        {
            int numBytesToRead;
            var buffer = new byte[65536]; // 128 Кб

            while ((numBytesToRead = await fs.ReadAsync(buffer)) > 0)
            {
                ReadOnlySpan<byte> span = buffer.AsSpan(0, numBytesToRead);

                foreach (byte b in span)
                {
                    if (b == ' ') ++spaceCount;
                }
            }
        }

        return spaceCount;
    }

    public static async Task<Dictionary<string, int>> Count(DirectoryInfo directory)
    {
        if (!directory.Exists) throw new DirectoryNotFoundException();

        ConcurrentDictionary<string, int> concurrentDictionary = new ConcurrentDictionary<string, int>();
        var tasks = new List<Task>();

        foreach (var file in directory.GetFiles())
        {
            var currentFile = file;
            
            tasks.Add(Task.Run(async () =>
            {
                try
                {
                    var count = await Count(currentFile);
                    concurrentDictionary[currentFile.Name] = count;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing file {currentFile.Name}: {ex.Message}");
                    concurrentDictionary[currentFile.Name] = -1;
                }
            }));
        }

        await Task.WhenAll(tasks);
        
        return new Dictionary<string, int>(concurrentDictionary);
    }
}