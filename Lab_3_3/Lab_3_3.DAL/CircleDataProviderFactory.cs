namespace Lab_3_3.DAL;

public static class CircleDataProviderFactory
{
    public static ICircleDataProvider Create(StorageFormat format) =>
        format switch
        {
            StorageFormat.Binary => new BinaryCircleDataProvider(),
            StorageFormat.Xml => new XmlCircleDataProvider(),
            StorageFormat.Json => new JsonCircleDataProvider(),
            StorageFormat.Custom => new CustomCircleDataProvider(),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, "Unknown storage format.")
        };
}
