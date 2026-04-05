using Homework07.Domain;
using Homework07.Serialization;

namespace Homework07.Tests;

public sealed class CsvReflectionSerializerTests
{
    private readonly CsvReflectionSerializer _serializer = new();

    [Fact]
    public void Serialize_ShouldReturnExpectedCsv_ForF()
    {
        var csv = _serializer.Serialize(F.Get());

        var expected = $"i1,i2,i3,i4,i5{Environment.NewLine}1,2,3,4,5";
        Assert.Equal(expected, csv);
    }

    [Fact]
    public void Deserialize_ShouldRestoreValues_ForF()
    {
        var csv = $"i1,i2,i3,i4,i5{Environment.NewLine}1,2,3,4,5";

        var model = _serializer.Deserialize<F>(csv);

        Assert.Equal(1, model.i1);
        Assert.Equal(2, model.i2);
        Assert.Equal(3, model.i3);
        Assert.Equal(4, model.i4);
        Assert.Equal(5, model.i5);
    }

    [Fact]
    public void Serialize_And_Deserialize_ShouldHandleEscapedValues()
    {
        var original = new CsvSample
        {
            Name = "A,B \"C\"",
            Count = 7
        };

        var csv = _serializer.Serialize(original);
        var restored = _serializer.Deserialize<CsvSample>(csv);

        Assert.Equal(original.Name, restored.Name);
        Assert.Equal(original.Count, restored.Count);
    }

    public sealed class CsvSample
    {
        public string Name { get; set; } = string.Empty;

        public int Count { get; set; }
    }
}
