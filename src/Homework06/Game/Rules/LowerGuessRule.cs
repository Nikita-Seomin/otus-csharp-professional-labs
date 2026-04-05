namespace Homework06.Game.Rules;

public sealed class LowerGuessRule : IGuessRule
{
    public bool CanHandle(int guess, int secret)
    {
        return guess < secret;
    }

    public GuessFeedback Evaluate(int guess, int secret)
    {
        return new GuessFeedback(false, "Больше.");
    }
}
