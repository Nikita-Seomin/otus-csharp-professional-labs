using Homework06.Game;

namespace Homework06.Infrastructure;

public sealed class ConsoleOutputWriter : IOutputWriter
{
    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }
}
