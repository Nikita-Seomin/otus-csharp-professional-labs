using Homework06.Configuration;

namespace Homework06.Tests;

public class GameSettingsTests
{
    [Fact]
    public void Load_Should_Read_Settings_From_Json_File()
    {
        var validator = new GameSettingsValidator();
        var settingsPath = CreateTempSettingsFile("""
        {
          "minNumber": 5,
          "maxNumber": 15,
          "maxAttempts": 4
        }
        """);

        try
        {
            var provider = new JsonGameSettingsProvider(settingsPath, validator);

            var settings = provider.Load();

            Assert.Equal(5, settings.MinNumber);
            Assert.Equal(15, settings.MaxNumber);
            Assert.Equal(4, settings.MaxAttempts);
        }
        finally
        {
            File.Delete(settingsPath);
        }
    }

    [Theory]
    [InlineData(10, 10, 3)]
    [InlineData(10, 5, 3)]
    [InlineData(1, 10, 0)]
    [InlineData(1, 10, -1)]
    public void Validate_Should_Throw_For_Invalid_Settings(int minNumber, int maxNumber, int maxAttempts)
    {
        var validator = new GameSettingsValidator();
        var settings = new GameSettings(minNumber, maxNumber, maxAttempts);

        Assert.Throws<InvalidOperationException>(() => validator.Validate(settings));
    }

    private static string CreateTempSettingsFile(string content)
    {
        var path = Path.Combine(Path.GetTempPath(), $"homework06-{Guid.NewGuid():N}.json");
        File.WriteAllText(path, content);
        return path;
    }
}
