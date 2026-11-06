namespace Lab_3_3.DAL;

public class CircleRepository
{
    public void Save(
        string filePath,
        StorageFormat format,
        IReadOnlyList<CircleEntity> circles)
    {
        ICircleDataProvider provider = CircleDataProviderFactory.Create(format);
        provider.Save(filePath, circles);
    }

    public List<CircleEntity> Load(string filePath, StorageFormat format)
    {
        ICircleDataProvider provider = CircleDataProviderFactory.Create(format);
        return provider.Load(filePath);
    }
}
