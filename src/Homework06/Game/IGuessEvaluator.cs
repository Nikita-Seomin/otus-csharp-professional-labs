namespace Homework06.Game;

public interface IGuessEvaluator
{
    GuessFeedback Evaluate(int guess, int secret);
}
