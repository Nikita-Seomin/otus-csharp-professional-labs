using Homework05.Domain;

namespace Homework05Tests;

public class LiveRecordingCloneTests
{
    [Fact]
    public void MyClone_Should_Create_DeepCopy_For_Collections()
    {
        // Arrange
        var original = CreateSample();
        original.SetMetadata("Source", "Audience");
        original.AddTag("live");

        // Act
        var clone = original.MyClone();
        clone.SetMetadata("Source", "Soundboard");
        clone.AddTag("remastered");

        // Assert: значения в оригинале не меняются
        Assert.Equal("Audience", original.Metadata["Source"]);
        Assert.DoesNotContain("remastered", original.Tags);

        // И при этом содержимое похоже, но коллекции — разные объекты
        Assert.NotSame(original.Metadata, clone.Metadata);
        Assert.NotSame(original.Tags, clone.Tags);
    }

    [Fact]
    public void MyClone_Should_Preserve_All_Fields()
    {
        // Arrange
        var original = CreateSample();

        // Act
        var clone = original.MyClone();

        // Assert
        Assert.Equal(original.Id, clone.Id);
        Assert.Equal(original.Title, clone.Title);
        Assert.Equal(original.Duration, clone.Duration);
        Assert.Equal(original.Codec, clone.Codec);
        Assert.Equal(original.Artist, clone.Artist);
        Assert.Equal(original.Album, clone.Album);
        Assert.Equal(original.Venue, clone.Venue);
        Assert.Equal(original.RecordingDate, clone.RecordingDate);
    }

    [Fact]
    public void ICloneable_Clone_Should_Use_MyClone()
    {
        // Arrange
        var original = CreateSample();

        // Act
        var cloneObj = ((ICloneable)original).Clone();
        var clone = Assert.IsType<LiveRecording>(cloneObj);

        // Assert
        Assert.Equal(original.Title, clone.Title);
    }

    private static LiveRecording CreateSample()
        => new LiveRecording(
            id: Guid.NewGuid(),
            title: "Everlong",
            duration: TimeSpan.FromMinutes(4.8),
            codec: "FLAC",
            artist: "Foo Fighters",
            album: "Live at Wembley",
            venue: "Wembley Stadium",
            recordingDate: new DateOnly(2008, 6, 7));
}