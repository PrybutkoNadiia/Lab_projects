using System.Xml.Serialization;

namespace Lab_3_3.DAL;

public class XmlCircleDataProvider : ICircleDataProvider
{
    private static readonly XmlSerializer Serializer = new(typeof(List<CircleEntity>));

    public void Save(string filePath, IReadOnlyList<CircleEntity> circles)
    {
        FilePathHelper.CreateParentDirectory(filePath);

        using var stream = File.Create(filePath);
        Serializer.Serialize(stream, circles.ToList());
    }

    public List<CircleEntity> Load(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        return Serializer.Deserialize(stream) as List<CircleEntity>
            ?? throw new InvalidDataException("The XML file does not contain a circle list.");
    }
}
