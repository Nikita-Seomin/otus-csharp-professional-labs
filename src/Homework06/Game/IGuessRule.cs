namespace Homework06.Game;

public interface IGuessRule
{
    bool CanHandle(int guess, int secret);

    GuessFeedback Evaluate(int guess, int secret);
}
