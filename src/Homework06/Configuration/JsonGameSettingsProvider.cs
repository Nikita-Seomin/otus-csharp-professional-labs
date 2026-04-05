using System.Text.Json;

namespace Homework06.Configuration;

public sealed class JsonGameSettingsProvider : IGameSettingsProvider
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly string _settingsPath;
    private readonly IGameSettingsValidator _settingsValidator;

    public JsonGameSettingsProvider(string settingsPath, IGameSettingsValidator settingsValidator)
    {
        _settingsPath = settingsPath;
        _settingsValidator = settingsValidator;
    }

    public GameSettings Load()
    {
        if (!File.Exists(_settingsPath))
        {
            throw new FileNotFoundException("Файл настроек не найден.", _settingsPath);
        }

        using var stream = File.OpenRead(_settingsPath);
        var settings = JsonSerializer.Deserialize<GameSettings>(stream, SerializerOptions)
                       ?? throw new InvalidOperationException("Не удалось прочитать настройки игры.");

        _settingsValidator.Validate(settings);
        return settings;
    }
}
