using Homework06.Configuration;
using Homework06.Game;
using Homework06.Game.Rules;
using Homework06.Infrastructure;

namespace Homework06;

public static class Program
{
    public static int Main()
    {
        var outputWriter = new ConsoleOutputWriter();
        var settingsPath = Path.Combine(AppContext.BaseDirectory, "gameSettings.json");

        try
        {
            var settingsProvider = new JsonGameSettingsProvider(settingsPath, new GameSettingsValidator());
            var settings = settingsProvider.Load();

            var guessGame = new GuessGame(
                settings,
                new ConsoleInputReader(),
                outputWriter,
                new RandomNumberGenerator(),
                new GuessEvaluator(
                    new IGuessRule[]
                    {
                        new CorrectGuessRule(),
                        new LowerGuessRule(),
                        new HigherGuessRule()
                    }));

            guessGame.Run();
            return 0;
        }
        catch (Exception exception)
        {
            outputWriter.WriteLine($"Ошибка запуска игры: {exception.Message}");
            return 1;
        }
    }
}
