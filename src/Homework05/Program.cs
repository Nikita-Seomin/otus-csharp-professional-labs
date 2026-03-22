// See https://aka.ms/new-console-template for more information

using Homework05.Domain;

internal static class Program
{
    private static void Main()
    {
        var original = new LiveRecording(
            id: Guid.NewGuid(),
            title: "Everlong",
            duration: TimeSpan.FromMinutes(4.8),
            codec: "FLAC",
            artist: "Foo Fighters",
            album: "Live at Wembley",
            venue: "Wembley Stadium",
            recordingDate: new DateOnly(2008, 6, 7));

        original.SetMetadata("Source", "Audience");
        original.SetMetadata("Bitrate", "Lossless");
        original.AddTag("rock");
        original.AddTag("live");

        // 1) Клонирование через наш интерфейс
        var clone1 = original.MyClone();

        // 2) Клонирование через ICloneable
        var clone2 = (LiveRecording)((ICloneable)original).Clone();

        // Меняем клоны — оригинал не должен измениться
        clone1.SetMetadata("Source", "Soundboard");
        clone1.AddTag("remastered");

        Console.WriteLine("ORIGINAL:");
        Print(original);

        Console.WriteLine("\nCLONE #1 (MyClone + modified):");
        Print(clone1);

        Console.WriteLine("\nCLONE #2 (ICloneable.Clone):");
        Print(clone2);
    }

    private static void Print(LiveRecording track)
    {
        Console.WriteLine(track);
        Console.WriteLine("Metadata:");
        foreach (var kv in track.Metadata)
            Console.WriteLine($"  {kv.Key}: {kv.Value}");

        Console.WriteLine("Tags: " + string.Join(", ", track.Tags));
    }
}