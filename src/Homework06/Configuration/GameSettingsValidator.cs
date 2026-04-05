namespace Homework06.Configuration;

public sealed class GameSettingsValidator : IGameSettingsValidator
{
    public void Validate(GameSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        if (settings.MinNumber >= settings.MaxNumber)
        {
            throw new InvalidOperationException("Минимальное значение диапазона должно быть меньше максимального.");
        }

        if (settings.MaxAttempts <= 0)
        {
            throw new InvalidOperationException("Количество попыток должно быть положительным.");
        }
    }
}
