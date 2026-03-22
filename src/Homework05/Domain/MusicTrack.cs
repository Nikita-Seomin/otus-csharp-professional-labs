namespace Homework05.Domain;

public abstract class MusicTrack : AudioFile, IMyCloneable<MusicTrack>
{
    public string Artist { get; }
    public string Album { get; }

    protected MusicTrack(Guid id, string title, TimeSpan duration, string codec, string artist, string album)
        : base(id, title, duration, codec)
    {
        Artist = artist ?? throw new ArgumentNullException(nameof(artist));
        Album = album ?? throw new ArgumentNullException(nameof(album));
    }

    protected MusicTrack(MusicTrack other)
        : base(other)
    {
        Artist = other.Artist;
        Album = other.Album;
    }

    public abstract override MusicTrack MyClone();
}