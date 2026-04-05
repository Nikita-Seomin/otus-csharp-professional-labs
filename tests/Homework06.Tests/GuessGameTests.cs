using Homework06.Configuration;
using Homework06.Game;
using Homework06.Game.Rules;
using Homework06.Infrastructure;

namespace Homework06.Tests;

public class GuessGameTests
{
    [Fact]
    public void Run_Should_End_With_Victory_When_Number_Is_Guessed()
    {
        var output = new FakeOutputWriter();
        var game = CreateGame(new[] { "10", "42" }, output, secretNumber: 42, maxAttempts: 5);

        game.Run();

        Assert.Contains(output.Messages, message => message.Contains("Больше."));
        Assert.Contains(output.Messages, message => message.Contains("Победа. Число угадано за 2 попыток."));
        Assert.DoesNotContain(output.Messages, message => message.Contains("Попытки закончились."));
    }

    [Fact]
    public void Run_Should_End_With_Loss_When_Attempts_Are_Over()
    {
        var output = new FakeOutputWriter();
        var game = CreateGame(new[] { "10", "20", "30" }, output, secretNumber: 42, maxAttempts: 3);

        game.Run();

        Assert.Contains(output.Messages, message => message.Contains("Попытки закончились. Было загадано число 42."));
    }

    [Fact]
    public void Run_Should_Ask_For_Input_Again_When_Value_Is_Invalid()
    {
        var output = new FakeOutputWriter();
        var game = CreateGame(new[] { "abc", "101", "42" }, output, secretNumber: 42, maxAttempts: 1);

        game.Run();

        Assert.Contains(output.Messages, message => message.Contains("Некорректный ввод. Нужно ввести целое число."));
        Assert.Contains(output.Messages, message => message.Contains("Число должно находиться в диапазоне"));
        Assert.Contains(output.Messages, message => message.Contains("Победа. Число угадано за 1 попыток."));
    }

    [Fact]
    public void FixedNumberGenerator_Should_Throw_When_Number_Is_Out_Of_Range()
    {
        var generator = new FixedNumberGenerator(100);

        Assert.Throws<InvalidOperationException>(() => generator.Generate(1, 10));
    }

    private static GuessGame CreateGame(IEnumerable<string> inputs, FakeOutputWriter output, int secretNumber, int maxAttempts)
    {
        return new GuessGame(
            new GameSettings(1, 100, maxAttempts),
            new FakeInputReader(inputs),
            output,
            new FixedNumberGenerator(secretNumber),
            new GuessEvaluator(
                new IGuessRule[]
                {
                    new CorrectGuessRule(),
                    new LowerGuessRule(),
                    new HigherGuessRule()
                }));
    }

    private sealed class FakeInputReader : IInputReader
    {
        private readonly Queue<string> _inputs;

        public FakeInputReader(IEnumerable<string> inputs)
        {
            _inputs = new Queue<string>(inputs);
        }

        public string? ReadLine()
        {
            return _inputs.Count > 0 ? _inputs.Dequeue() : null;
        }
    }

    private sealed class FakeOutputWriter : IOutputWriter
    {
        public List<string> Messages { get; } = new();

        public void WriteLine(string message)
        {
            Messages.Add(message);
        }
    }
}
