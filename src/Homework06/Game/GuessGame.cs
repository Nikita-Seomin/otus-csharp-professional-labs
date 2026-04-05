using Homework06.Configuration;

namespace Homework06.Game;

public sealed class GuessGame
{
    private readonly GameSettings _settings;
    private readonly IInputReader _inputReader;
    private readonly IOutputWriter _outputWriter;
    private readonly INumberGenerator _numberGenerator;
    private readonly IGuessEvaluator _guessEvaluator;

    public GuessGame(
        GameSettings settings,
        IInputReader inputReader,
        IOutputWriter outputWriter,
        INumberGenerator numberGenerator,
        IGuessEvaluator guessEvaluator)
    {
        _settings = settings;
        _inputReader = inputReader;
        _outputWriter = outputWriter;
        _numberGenerator = numberGenerator;
        _guessEvaluator = guessEvaluator;
    }

    public void Run()
    {
        var secret = _numberGenerator.Generate(_settings.MinNumber, _settings.MaxNumber);

        _outputWriter.WriteLine(
            $"Я загадал число от {_settings.MinNumber} до {_settings.MaxNumber}. У тебя {_settings.MaxAttempts} попыток.");

        for (var attempt = 1; attempt <= _settings.MaxAttempts; attempt++)
        {
            var guess = ReadGuess(attempt);
            var feedback = _guessEvaluator.Evaluate(guess, secret);

            _outputWriter.WriteLine(feedback.Message);

            if (feedback.IsCorrect)
            {
                _outputWriter.WriteLine($"Победа. Число угадано за {attempt} попыток.");
                return;
            }
        }

        _outputWriter.WriteLine($"Попытки закончились. Было загадано число {secret}.");
    }

    private int ReadGuess(int attempt)
    {
        while (true)
        {
            _outputWriter.WriteLine(
                $"Попытка {attempt} из {_settings.MaxAttempts}. Введи целое число от {_settings.MinNumber} до {_settings.MaxNumber}:");

            var input = _inputReader.ReadLine();

            if (!int.TryParse(input, out var guess))
            {
                _outputWriter.WriteLine("Некорректный ввод. Нужно ввести целое число.");
                continue;
            }

            if (guess < _settings.MinNumber || guess > _settings.MaxNumber)
            {
                _outputWriter.WriteLine(
                    $"Число должно находиться в диапазоне от {_settings.MinNumber} до {_settings.MaxNumber}.");
                continue;
            }

            return guess;
        }
    }
}
