namespace Homework06.Game.Rules;

public sealed class CorrectGuessRule : IGuessRule
{
    public bool CanHandle(int guess, int secret)
    {
        return guess == secret;
    }

    public GuessFeedback Evaluate(int guess, int secret)
    {
        return new GuessFeedback(true, "Верно.");
    }
}
