using System.Diagnostics;

namespace Homework03;

public class Program
{
    public static async Task Main(string[] args)
    {
        #region 1 - 3 файла параллельно

        var tasks = new List<Task>();

        string[] arr =
        [
            "../../../Sample3Files/1.txt",
            "../../../Sample3Files/2.txt",
            "../../../Sample3Files/3.txt"
        ];

        Stopwatch stopwatch1 = new Stopwatch();

        stopwatch1.Start();
        foreach (var item in arr)
        {
            var currentPath = item;
            
            tasks.Add(Task.Run( async () =>
            {
                var spaceCount = await CounterSpace.Count(new FileInfo(currentPath));
                Console.WriteLine($"В файле {currentPath} найдено {spaceCount} пробелов");
            }));
        }

        await Task.WhenAll(tasks);

        stopwatch1.Stop();
        Console.WriteLine($"Время выполнения: {stopwatch1.ElapsedMilliseconds} мс\n");

        #endregion

        #region 2 - Все файлы в папке
        Stopwatch stopwatch2 = new Stopwatch();
        
        stopwatch2.Start();
        var spaceCountDic2 =  await CounterSpace.Count(new DirectoryInfo("../../../Sample3Files"));
        stopwatch2.Stop();
        
        foreach (var item in spaceCountDic2)
            Console.WriteLine($"В файле {item.Key} найдено {item.Value} пробелов");
        
        Console.WriteLine($"Время выполнения: {stopwatch2.ElapsedMilliseconds} мс\n");
        #endregion
    }
}