using Homework06.Game;
using Homework06.Game.Rules;

namespace Homework06.Tests;

public class GuessEvaluatorTests
{
    private static readonly GuessEvaluator Evaluator = new(
        new IGuessRule[]
        {
            new CorrectGuessRule(),
            new LowerGuessRule(),
            new HigherGuessRule()
        });

    [Theory]
    [InlineData(50, 50, true, "Верно.")]
    [InlineData(10, 50, false, "Больше.")]
    [InlineData(90, 50, false, "Меньше.")]
    public void Evaluate_Should_Return_Expected_Feedback(int guess, int secret, bool isCorrect, string message)
    {
        var result = Evaluator.Evaluate(guess, secret);

        Assert.Equal(isCorrect, result.IsCorrect);
        Assert.Equal(message, result.Message);
    }
}
