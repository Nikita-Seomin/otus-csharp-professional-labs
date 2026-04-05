using System.Diagnostics;
using System.Text.Json;
using Homework07.Domain;
using Homework07.Serialization;

namespace Homework07.Benchmark;

public sealed class SerializationBenchmark(CsvReflectionSerializer csvSerializer)
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        IncludeFields = true
    };

    public BenchmarkResult Run(F source, int iterations)
    {
        ArgumentNullException.ThrowIfNull(source);

        if (iterations <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(iterations), iterations, " оличество итераций должно быть больше нул€.");
        }

        Warmup(source);

        var csvPayload = csvSerializer.Serialize(source);
        var jsonPayload = JsonSerializer.Serialize(source, _jsonOptions);

        var reflectionSerializationMs = Measure(() =>
        {
            for (var i = 0; i < iterations; i++)
            {
                csvSerializer.Serialize(source);
            }
        });

        var reflectionDeserializationMs = Measure(() =>
        {
            for (var i = 0; i < iterations; i++)
            {
                csvSerializer.Deserialize<F>(csvPayload);
            }
        });

        var jsonSerializationMs = Measure(() =>
        {
            for (var i = 0; i < iterations; i++)
            {
                JsonSerializer.Serialize(source, _jsonOptions);
            }
        });

        var jsonDeserializationMs = Measure(() =>
        {
            for (var i = 0; i < iterations; i++)
            {
                var value = JsonSerializer.Deserialize<F>(jsonPayload, _jsonOptions);
                if (value is null)
                {
                    throw new InvalidOperationException("System.Text.Json вернул null при десериализации.");
                }
            }
        });

        var consoleWriteMs = Measure(() =>
        {
            Console.WriteLine($"ѕолученна€ CSV-строка:{Environment.NewLine}{csvPayload}");
        });

        return new BenchmarkResult(
            iterations,
            csvPayload,
            jsonPayload,
            reflectionSerializationMs,
            reflectionDeserializationMs,
            jsonSerializationMs,
            jsonDeserializationMs,
            consoleWriteMs);
    }

    private void Warmup(F source)
    {
        var csvPayload = csvSerializer.Serialize(source);
        csvSerializer.Deserialize<F>(csvPayload);

        var jsonPayload = JsonSerializer.Serialize(source, _jsonOptions);
        JsonSerializer.Deserialize<F>(jsonPayload, _jsonOptions);
    }

    private static long Measure(Action action)
    {
        var stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();
        return stopwatch.ElapsedMilliseconds;
    }
}
