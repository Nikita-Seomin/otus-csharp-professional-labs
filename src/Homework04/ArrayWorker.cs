namespace Homework04;

public static class ArrayWorker
{
    public static long Sum(IEnumerable<int> numbers)
    {
        long result = 0;
        foreach (var num in numbers)
            result += num;
        return result;
    }

    public static long SumParallel(int[] numbers, int? threadsCount = null)
    {
        ArgumentNullException.ThrowIfNull(numbers);

        var workers = threadsCount.GetValueOrDefault(Environment.ProcessorCount);
        if (workers < 1) workers = 1;
        if (workers > numbers.Length) workers = numbers.Length;

        var partial = new long[workers];
        var threads = new Thread[workers];

        var chunkSize = numbers.Length / workers;
        var remainder = numbers.Length % workers;

        var start = 0;
        for (var t = 0; t < workers; t++)
        {
            var extra = (t < remainder) ? 1 : 0;
            var from = start;
            var to = start + chunkSize + extra; 

            var idx = t;
            threads[t] = new Thread(() =>
            {
                long local = 0;
                for (var i = from; i < to; i++)
                    local += numbers[i];

                partial[idx] = local;
            });

            threads[t].IsBackground = true;
            threads[t].Start();

            start = to;
        }

        for (var t = 0; t < workers; t++)
            threads[t].Join();

        long total = 0;
        for (var t = 0; t < workers; t++)
            total += partial[t];

        return total;
    }
    
    public static long SumLinq(int[] numbers)
    {
        return numbers.AsParallel().Sum(x => (long)x);
    }
    
    public static int[] FillInt(int value)
    {
        var rand = new Random();
        int[] res = new int[value];
        for (int ctr = 0; ctr < value; ctr++)
            res[ctr] = rand.Next();

        return res;
    }
}