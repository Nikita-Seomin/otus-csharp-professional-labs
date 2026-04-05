namespace Homework08.FileSearch;

public sealed class FileArgs : EventArgs
{
    public FileArgs(string fileName)
    {
        FileName = fileName;
    }

    public string FileName { get; }

    public bool Cancel { get; set; }
}
