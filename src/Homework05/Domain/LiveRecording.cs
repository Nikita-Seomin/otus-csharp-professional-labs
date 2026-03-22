namespace Homework05.Domain;

public sealed class LiveRecording : MusicTrack, IMyCloneable<LiveRecording>
{
    public string Venue { get; }
    public DateOnly RecordingDate { get; }

    public LiveRecording(
        Guid id,
        string title,
        TimeSpan duration,
        string codec,
        string artist,
        string album,
        string venue,
        DateOnly recordingDate)
        : base(id, title, duration, codec, artist, album)
    {
        Venue = venue ?? throw new ArgumentNullException(nameof(venue));
        RecordingDate = recordingDate;
    }

    /// <summary>
    /// Copy-конструктор leaf-класса: вызывает родительский copy-конструктор.
    /// </summary>
    private LiveRecording(LiveRecording other)
        : base(other)
    {
        Venue = other.Venue;
        RecordingDate = other.RecordingDate;
    }

    public override LiveRecording MyClone() => new(this);

    // Дополнительно: чтобы было явно, что интерфейс IMyCloneable<LiveRecording> реализован.
    LiveRecording IMyCloneable<LiveRecording>.MyClone() => MyClone();

    public override string ToString()
        => $"{base.ToString()}, {Artist} — {Title} @ {Venue} ({RecordingDate})";
}