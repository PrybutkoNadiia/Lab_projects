using System.Globalization;
using System.Text;

namespace Lab_3_3.DAL;

public class CustomCircleDataProvider : ICircleDataProvider
{
    private const string FileHeader = "CIRCLES|1";

    public void Save(string filePath, IReadOnlyList<CircleEntity> circles)
    {
        FilePathHelper.CreateParentDirectory(filePath);

        using var writer = new StreamWriter(filePath, false, Encoding.UTF8);
        writer.WriteLine(FileHeader);

        foreach (CircleEntity circle in circles)
        {
            writer.WriteLine(
                $"{Escape(circle.FillColor)}|{Escape(circle.OutlineColor)}|{circle.Radius.ToString("R", CultureInfo.InvariantCulture)}");
        }
    }

    public List<CircleEntity> Load(string filePath)
    {
        using var reader = new StreamReader(filePath, Encoding.UTF8);
        if (reader.ReadLine() != FileHeader)
        {
            throw new InvalidDataException("The custom file has an invalid header.");
        }

        var circles = new List<CircleEntity>();
        string? line;
        int lineNumber = 1;

        while ((line = reader.ReadLine()) is not null)
        {
            lineNumber++;
            string[] parts = line.Split('|');
            if (parts.Length != 3 ||
                !double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out double radius))
            {
                throw new InvalidDataException($"Invalid circle data on line {lineNumber}.");
            }

            try
            {
                circles.Add(new CircleEntity
                {
                    FillColor = Uri.UnescapeDataString(parts[0]),
                    OutlineColor = Uri.UnescapeDataString(parts[1]),
                    Radius = radius
                });
            }
            catch (UriFormatException exception)
            {
                throw new InvalidDataException($"Invalid color data on line {lineNumber}.", exception);
            }
        }

        return circles;
    }

    private static string Escape(string value) => Uri.EscapeDataString(value);
}
