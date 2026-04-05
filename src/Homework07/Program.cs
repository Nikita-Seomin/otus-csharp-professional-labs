using Homework07.Benchmark;
using Homework07.Domain;
using Homework07.Serialization;

namespace Homework07;

public static class Program
{
    public static int Main()
    {
        const int iterations = 100_000;

        try
        {
            var benchmark = new SerializationBenchmark(new CsvReflectionSerializer());
            var result = benchmark.Run(F.Get(), iterations);

            Console.WriteLine();
            Console.WriteLine("Сериализуемый класс: class F { int i1, i2, i3, i4, i5; }");
            Console.WriteLine($"Количество замеров: {result.Iterations} итераций");
            Console.WriteLine();
            Console.WriteLine("мой рефлекшен:");
            Console.WriteLine($"Время на сериализацию = {result.ReflectionSerializationMs} мс");
            Console.WriteLine($"Время на десериализацию = {result.ReflectionDeserializationMs} мс");
            Console.WriteLine();
            Console.WriteLine("стандартный механизм (System.Text.Json):");
            Console.WriteLine($"Время на сериализацию = {result.JsonSerializationMs} мс");
            Console.WriteLine($"Время на десериализацию = {result.JsonDeserializationMs} мс");
            Console.WriteLine();
            Console.WriteLine($"Время на вывод CSV в консоль = {result.ConsoleWriteMs} мс");
            Console.WriteLine($"JSON для сравнения: {result.JsonPayload}");

            return 0;
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Ошибка выполнения: {exception.Message}");
            return 1;
        }
    }
}
