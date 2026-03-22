namespace Homework05.Domain;

public abstract class MediaFile : IMyCloneable<MediaFile>, ICloneable
{
    private readonly Dictionary<string, string> _metadata;
    private readonly List<string> _tags;

    public Guid Id { get; }
    public string Title { get; }

    public IReadOnlyDictionary<string, string> Metadata => _metadata;
    public IReadOnlyList<string> Tags => _tags;

    protected MediaFile(Guid id, string title)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id must not be empty.", nameof(id));
        Title = title ?? throw new ArgumentNullException(nameof(title));

        Id = id;
        _metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        _tags = new List<string>();
    }

    /// <summary>
    /// Copy-конструктор базового класса: копирует общие поля (глубоко)
    /// </summary>
    protected MediaFile(MediaFile other)
    {
        if (other is null) throw new ArgumentNullException(nameof(other));

        Id = other.Id;
        Title = other.Title;

        _metadata = new Dictionary<string, string>(other._metadata, StringComparer.OrdinalIgnoreCase);
        _tags = new List<string>(other._tags);
    }

    public void SetMetadata(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key is required.", nameof(key));
        _metadata[key] = value ?? string.Empty;
    }

    public void AddTag(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag)) throw new ArgumentException("Tag is required.", nameof(tag));
        _tags.Add(tag);
    }

    public abstract MediaFile MyClone();

    object ICloneable.Clone() => MyClone();

    public override string ToString()
        => $"{GetType().Name}: {Title} ({Id})";
}