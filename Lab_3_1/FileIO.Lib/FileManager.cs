using System.Text;

namespace FileIO.Lib;

public interface ITextFileStore
{
    string ReadAll(string path);
    void WriteAll(string path, string content);
}

public sealed class FileManager : ITextFileStore
{
    public string ReadAll(string path)
    {
        if (!File.Exists(path))
        {
            return string.Empty;
        }

        using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        return reader.ReadToEnd();
    }

    public void WriteAll(string path, string content)
    {
        using FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        using StreamWriter writer = new StreamWriter(stream, new UTF8Encoding(false));
        writer.Write(content);
    }
}
