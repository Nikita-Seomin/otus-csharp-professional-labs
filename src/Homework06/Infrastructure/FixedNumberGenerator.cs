using Homework06.Game;

namespace Homework06.Infrastructure;

public sealed class FixedNumberGenerator : INumberGenerator
{
    private readonly int _value;

    public FixedNumberGenerator(int value)
    {
        _value = value;
    }

    public int Generate(int minInclusive, int maxInclusive)
    {
        if (_value < minInclusive || _value > maxInclusive)
        {
            throw new InvalidOperationException("Фиксированное число выходит за границы диапазона игры.");
        }

        return _value;
    }
}
