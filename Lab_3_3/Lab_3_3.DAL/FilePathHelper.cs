namespace Lab_3_3.DAL;

internal static class FilePathHelper
{
    public static void CreateParentDirectory(string filePath)
    {
        string fullPath = Path.GetFullPath(filePath);
        string? directory = Path.GetDirectoryName(fullPath);

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}
