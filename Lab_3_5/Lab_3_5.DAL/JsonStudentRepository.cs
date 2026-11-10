using System.Text.Json;
using System.Text.Json.Serialization;
using Lab_3_5.BLL;

namespace Lab_3_5.DAL;

public sealed class JsonStudentRepository : IStudentRepository
{
    private static readonly JsonSerializerOptions JsonOptions = CreateJsonOptions();
    private readonly string _filePath;

    public JsonStudentRepository(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("The file path cannot be empty.", nameof(filePath));
        }

        _filePath = Path.GetFullPath(filePath);
    }

    public IReadOnlyList<Student> LoadAll()
    {
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException("The student file was not found.", _filePath);
        }

        string json = File.ReadAllText(_filePath);

        try
        {
            return JsonSerializer.Deserialize<List<Student>>(json, JsonOptions)
                ?? throw new InvalidDataException("The student file contains no data.");
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException(
                $"The student file '{_filePath}' contains invalid JSON.",
                exception);
        }
    }

    public void SaveAll(IReadOnlyList<Student> students)
    {
        ArgumentNullException.ThrowIfNull(students);

        if (students.Any(student => student is null))
        {
            throw new ArgumentException("The collection cannot contain null students.", nameof(students));
        }

        string? directory = Path.GetDirectoryName(_filePath);
        if (directory is null)
        {
            throw new InvalidOperationException("The student file must have a parent directory.");
        }

        Directory.CreateDirectory(directory);
        string json = JsonSerializer.Serialize(students, JsonOptions);
        File.WriteAllText(_filePath, json);
    }

    private static JsonSerializerOptions CreateJsonOptions()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        options.Converters.Add(new JsonStringEnumConverter());
        return options;
    }
}
