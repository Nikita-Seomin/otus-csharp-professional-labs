namespace Homework07.Benchmark;

public sealed record BenchmarkResult(
    int Iterations,
    string CsvPayload,
    string JsonPayload,
    long ReflectionSerializationMs,
    long ReflectionDeserializationMs,
    long JsonSerializationMs,
    long JsonDeserializationMs,
    long ConsoleWriteMs);
