namespace Homework08.FileSearch;

public sealed class FileSearcher
{
    public event EventHandler? FileFound;

    public IReadOnlyCollection<string> Search(string directoryPath, string searchPattern = "*")
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
        {
            throw new ArgumentException("Путь к каталогу не должен быть пустым.", nameof(directoryPath));
        }

        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException($"Каталог не найден: {directoryPath}");
        }

        var foundFiles = new List<string>();

        foreach (var filePath in Directory.EnumerateFiles(directoryPath, searchPattern, SearchOption.AllDirectories))
        {
            foundFiles.Add(filePath);

            var eventArgs = new FileArgs(Path.GetFileName(filePath));
            OnFileFound(eventArgs);

            if (eventArgs.Cancel)
            {
                break;
            }
        }

        return foundFiles;
    }

    private void OnFileFound(FileArgs eventArgs)
    {
        FileFound?.Invoke(this, eventArgs);
    }
}