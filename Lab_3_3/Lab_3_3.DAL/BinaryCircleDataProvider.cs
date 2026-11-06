using System.Text;

namespace Lab_3_3.DAL;

public class BinaryCircleDataProvider : ICircleDataProvider
{
    private const string FileSignature = "CIRCLES";
    private const int FileVersion = 1;

    public void Save(string filePath, IReadOnlyList<CircleEntity> circles)
    {
        FilePathHelper.CreateParentDirectory(filePath);

        using var stream = File.Create(filePath);
        using var writer = new BinaryWriter(stream, Encoding.UTF8);

        writer.Write(FileSignature);
        writer.Write(FileVersion);
        writer.Write(circles.Count);

        foreach (CircleEntity circle in circles)
        {
            writer.Write(circle.FillColor);
            writer.Write(circle.OutlineColor);
            writer.Write(circle.Radius);
        }
    }

    public List<CircleEntity> Load(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        using var reader = new BinaryReader(stream, Encoding.UTF8);

        if (reader.ReadString() != FileSignature || reader.ReadInt32() != FileVersion)
        {
            throw new InvalidDataException("The binary file has an invalid signature or version.");
        }

        int count = reader.ReadInt32();
        long minimumBytesPerCircle = 2 + sizeof(double);
        if (count < 0 || count > (stream.Length - stream.Position) / minimumBytesPerCircle)
        {
            throw new InvalidDataException("The binary file contains an invalid circle count.");
        }

        var circles = new List<CircleEntity>(count);
        for (int i = 0; i < count; i++)
        {
            circles.Add(new CircleEntity
            {
                FillColor = reader.ReadString(),
                OutlineColor = reader.ReadString(),
                Radius = reader.ReadDouble()
            });
        }

        if (stream.Position != stream.Length)
        {
            throw new InvalidDataException("The binary file contains unexpected extra data.");
        }

        return circles;
    }
}
