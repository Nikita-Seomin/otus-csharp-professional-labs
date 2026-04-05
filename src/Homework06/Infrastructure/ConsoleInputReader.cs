using Homework06.Game;

namespace Homework06.Infrastructure;

public sealed class ConsoleInputReader : IInputReader
{
    public string? ReadLine()
    {
        return Console.ReadLine();
    }
}
