using System.Diagnostics;
using Homework04;

public class Program
{
    public static void Main(string[] args)
    {
        List<int[]> testingArrays =
        [
            ArrayWorker.FillInt(100_000),
            ArrayWorker.FillInt(1_000_000),
            ArrayWorker.FillInt(10_000_000)
        ];
        
        // Прогрев для JIT компиляции
        foreach (var a in testingArrays)
        {
            ArrayWorker.Sum(a);
            ArrayWorker.SumParallel(a);
            ArrayWorker.SumLinq(a);
        }

        foreach (var array in testingArrays)
        {
            // Синхронный метод
            var start = Stopwatch.GetTimestamp();
            var sum = ArrayWorker.Sum(array);
            var elapsed = Stopwatch.GetElapsedTime(start);
            Console.WriteLine($"Синхронный метод на {array.Length} элементов вычислил сумму {sum} за {elapsed.TotalMilliseconds} ms");
            
            // Асинхронный метод
            var startAsync = Stopwatch.GetTimestamp();
            var sumAsync = ArrayWorker.SumParallel(array);
            var elapsedAsync = Stopwatch.GetElapsedTime(startAsync);
            Console.WriteLine($"Асинхронный метод на {array.Length} элементов вычислил сумму {sumAsync} за {elapsedAsync.TotalMilliseconds} ms");
            
            // Linq метод
            var startLinq = Stopwatch.GetTimestamp();
            var sumLinq = ArrayWorker.SumLinq(array);
            var elapsedLinq = Stopwatch.GetElapsedTime(startLinq);
            Console.WriteLine($"LINQ метод на {array.Length} элементов вычислил сумму {sumLinq} за {elapsedLinq.TotalMilliseconds} ms\n");
        }
    }
}