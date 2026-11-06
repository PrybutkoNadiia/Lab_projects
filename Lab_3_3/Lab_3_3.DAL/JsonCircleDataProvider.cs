using System.Text.Json;

namespace Lab_3_3.DAL;

public class JsonCircleDataProvider : ICircleDataProvider
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    public void Save(string filePath, IReadOnlyList<CircleEntity> circles)
    {
        FilePathHelper.CreateParentDirectory(filePath);

        using var stream = File.Create(filePath);
        JsonSerializer.Serialize(stream, circles, Options);
    }

    public List<CircleEntity> Load(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        return JsonSerializer.Deserialize<List<CircleEntity>>(stream)
            ?? throw new InvalidDataException("The JSON file does not contain a circle list.");
    }
}
