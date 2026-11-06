using System.Text.Json;
using System.Xml;
using Lab_3_3.DAL;

namespace Lab_3_3.BLL;

public class CircleService
{
    private readonly CircleRepository _repository;

    public CircleService() : this(new CircleRepository())
    {
    }

    public CircleService(CircleRepository repository)
    {
        _repository = repository;
    }

    public List<Circle> GetAll(string filePath, CircleFileFormat format)
    {
        string fullPath = GetFullPath(filePath);
        if (!File.Exists(fullPath))
        {
            return [];
        }

        try
        {
            List<CircleEntity> entities = _repository.Load(fullPath, ToStorageFormat(format));
            if (entities.Any(entity => entity is null))
            {
                throw new InvalidDataException("The file contains an empty circle entry.");
            }

            var circles = entities.Select(ToCircle).ToList();
            circles.ForEach(Validate);
            return circles;
        }
        catch (Exception exception) when (IsStorageException(exception))
        {
            throw CreateStorageException(fullPath, exception);
        }
    }

    public void SaveAll(
        string filePath,
        CircleFileFormat format,
        IReadOnlyList<Circle> circles)
    {
        string fullPath = GetFullPath(filePath);
        foreach (Circle circle in circles)
        {
            Validate(circle);
        }

        try
        {
            _repository.Save(
                fullPath,
                ToStorageFormat(format),
                circles.Select(ToEntity).ToList());
        }
        catch (Exception exception) when (IsStorageException(exception))
        {
            throw CreateStorageException(fullPath, exception);
        }
    }

    public void Add(string filePath, CircleFileFormat format, Circle circle)
    {
        Validate(circle);
        List<Circle> circles = GetAll(filePath, format);
        circles.Add(circle);
        SaveAll(filePath, format, circles);
    }

    public void RemoveAt(string filePath, CircleFileFormat format, int index)
    {
        List<Circle> circles = GetAll(filePath, format);
        if (index < 0 || index >= circles.Count)
        {
            throw new CircleServiceException("There is no circle with that number.");
        }

        circles.RemoveAt(index);
        SaveAll(filePath, format, circles);
    }

    public SerializationDemoResult RunSerializationDemo(
        string filePath,
        CircleFileFormat format)
    {
        List<Circle> samples = CreateSampleCircles();

        Circle[] originalArray = samples.ToArray();
        SaveAll(filePath, format, originalArray);
        Circle[] restoredArray = GetAll(filePath, format).ToArray();

        List<Circle> originalList = samples.ToList();
        SaveAll(filePath, format, originalList);
        List<Circle> restoredList = GetAll(filePath, format);

        return new SerializationDemoResult(
            samples.Count,
            originalArray.SequenceEqual(restoredArray),
            originalList.SequenceEqual(restoredList));
    }

    public static List<Circle> CreateSampleCircles() =>
    [
        new("Red", "Black", 1),
        new("Blue", "White", 2),
        new("Green", "Black", 3),
        new("Yellow", "Gray", 4),
        new("Purple", "White", 5),
        new("Orange", "Black", 6)
    ];

    private static string GetFullPath(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new CircleServiceException("The file name cannot be empty.");
        }

        try
        {
            return Path.GetFullPath(filePath);
        }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or PathTooLongException)
        {
            throw new CircleServiceException("The file path is invalid.", exception);
        }
    }

    private static Circle ToCircle(CircleEntity entity) =>
        new(entity.FillColor, entity.OutlineColor, entity.Radius);

    private static CircleEntity ToEntity(Circle circle) =>
        new()
        {
            FillColor = circle.FillColor,
            OutlineColor = circle.OutlineColor,
            Radius = circle.Radius
        };

    private static void Validate(Circle circle)
    {
        if (string.IsNullOrWhiteSpace(circle.FillColor) ||
            string.IsNullOrWhiteSpace(circle.OutlineColor))
        {
            throw new CircleServiceException("Circle colors cannot be empty.");
        }

        if (!double.IsFinite(circle.Radius) || circle.Radius <= 0)
        {
            throw new CircleServiceException("The radius must be a positive number.");
        }
    }

    private static StorageFormat ToStorageFormat(CircleFileFormat format) =>
        format switch
        {
            CircleFileFormat.Binary => StorageFormat.Binary,
            CircleFileFormat.Xml => StorageFormat.Xml,
            CircleFileFormat.Json => StorageFormat.Json,
            CircleFileFormat.Custom => StorageFormat.Custom,
            _ => throw new CircleServiceException("The selected file format is not supported.")
        };

    private static bool IsStorageException(Exception exception) =>
        exception is IOException
            or UnauthorizedAccessException
            or ArgumentException
            or NotSupportedException
            or InvalidOperationException
            or XmlException
            or JsonException;

    private static CircleServiceException CreateStorageException(
        string filePath,
        Exception exception) =>
        new($"Could not read or write '{filePath}': {exception.Message}", exception);
}
