namespace Homework06.Game;

public sealed class GuessEvaluator : IGuessEvaluator
{
    private readonly IReadOnlyCollection<IGuessRule> _rules;

    public GuessEvaluator(IEnumerable<IGuessRule> rules)
    {
        _rules = rules.ToArray();
    }

    public GuessFeedback Evaluate(int guess, int secret)
    {
        foreach (var rule in _rules)
        {
            if (rule.CanHandle(guess, secret))
            {
                return rule.Evaluate(guess, secret);
            }
        }

        throw new InvalidOperationException("Не найдено правило для обработки введённого числа.");
    }
}
