namespace Lab_3_3.DAL;

public interface ICircleDataProvider
{
    void Save(string filePath, IReadOnlyList<CircleEntity> circles);
    List<CircleEntity> Load(string filePath);
}
