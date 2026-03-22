namespace Homework05.Domain;

public abstract class AudioFile : MediaFile, IMyCloneable<AudioFile>
{
    public TimeSpan Duration { get; }
    public string Codec { get; }

    protected AudioFile(Guid id, string title, TimeSpan duration, string codec)
        : base(id, title)
    {
        if (duration < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(duration));
        Codec = codec ?? throw new ArgumentNullException(nameof(codec));

        Duration = duration;
    }

    /// <summary>
    /// Copy-конструктор: обязательно вызывает родительский copy-конструктор.
    /// </summary>
    protected AudioFile(AudioFile other)
        : base(other)
    {
        Duration = other.Duration;
        Codec = other.Codec;
    }

    public abstract override AudioFile MyClone();
}