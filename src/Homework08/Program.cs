using Homework08.Extensions;
using Homework08.FileSearch;

namespace Homework08;

public static class Program
{
    public static int Main()
    {
        try
        {
            ShowGetMaxDemo();
            Console.WriteLine(new string('-', 60));
            ShowFileSearchDemo();

            return 0;
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Ошибка выполнения: {exception.Message}");
            return 1;
        }
    }

    private static void ShowGetMaxDemo()
    {
        var players = new List<Player>
        {
            new("Алексей", 72.4f),
            new("Мария", 91.8f),
            new("Ирина", 88.1f),
            new("Дмитрий", 79.5f)
        };

        var bestPlayer = players.GetMax(player => player.Rating);

        Console.WriteLine("Демонстрация GetMax:");
        Console.WriteLine($"Максимальный элемент: {bestPlayer.Name}, рейтинг: {bestPlayer.Rating}");
    }

    private static void ShowFileSearchDemo()
    {
        var searchDirectory = Path.Combine(Directory.GetCurrentDirectory(), "src");
        var searchPattern = "*.cs";
        var fileSearcher = new FileSearcher();
        var eventCounter = 0;

        fileSearcher.FileFound += (_, args) =>
        {
            if (args is not FileArgs fileArgs)
            {
                return;
            }

            eventCounter++;
            Console.WriteLine($"Событие FileFound: найден файл '{fileArgs.FileName}'");

            if (eventCounter >= 5)
            {
                fileArgs.Cancel = true;
                Console.WriteLine("Обработчик запросил отмену дальнейшего поиска.");
            }
        };

        Console.WriteLine("Демонстрация событий при обходе каталога:");
        Console.WriteLine($"Каталог: {searchDirectory}");
        Console.WriteLine($"Маска: {searchPattern}");

        var foundFiles = fileSearcher.Search(searchDirectory, searchPattern);

        Console.WriteLine($"Итог: найдено файлов до остановки: {foundFiles.Count}");
    }

    private sealed class Player
    {
        public Player(string name, float rating)
        {
            Name = name;
            Rating = rating;
        }

        public string Name { get; }

        public float Rating { get; }
    }
}