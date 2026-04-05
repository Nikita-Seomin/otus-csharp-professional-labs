# Homework 07

## Задание: рефлексия и сериализация

Сериализуемый класс:

```csharp
class F { int i1, i2, i3, i4, i5; }
```

Реализованный в проекте вариант:

```csharp
public sealed class F
{
    public int i1;
    public int i2;
    public int i3;
    public int i4;
    public int i5;
}
```

Код сериализации/десериализации:

- CSV reflection serializer: `src/Homework07/Serialization/CsvReflectionSerializer.cs`
- Benchmark запуск и замеры: `src/Homework07/Benchmark/SerializationBenchmark.cs`
- Точка входа: `src/Homework07/Program.cs`

Количество замеров:

- `100000` итераций

Среда:

- ОС: Windows
- Платформа: .NET `10.0`
- Запуск: `dotnet run --project src/Homework07/Homework07.csproj`

Результаты (фактический запуск):

мой рефлекшен:

- Время на сериализацию = `100 мс`
- Время на десериализацию = `191 мс`

стандартный механизм (`System.Text.Json`):

- Время на сериализацию = `69 мс`
- Время на десериализацию = `162 мс`

Дополнительно:

- Время на вывод CSV в консоль = `1 мс`
- Полученная CSV-строка:

```csv
i1,i2,i3,i4,i5
1,2,3,4,5
```
