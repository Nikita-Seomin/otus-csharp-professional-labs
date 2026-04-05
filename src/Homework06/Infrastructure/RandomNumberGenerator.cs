using Homework06.Game;

namespace Homework06.Infrastructure;

public sealed class RandomNumberGenerator : INumberGenerator
{
    private readonly Random _random = Random.Shared;

    public int Generate(int minInclusive, int maxInclusive)
    {
        return _random.Next(minInclusive, maxInclusive + 1);
    }
}
