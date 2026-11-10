using Lab_3_5.BLL;
using Lab_3_5.DAL;

namespace Lab_3_5.Tests;

public sealed class JsonStudentRepositoryTests : IDisposable
{
    private readonly string _directory =
        Path.Combine(Path.GetTempPath(), $"Lab_3_5.Tests-{Guid.NewGuid():N}");

    [Fact]
    public void SaveAllAndLoadAll_RoundTripStudentsAndAdditionalEntities()
    {
        string filePath = Path.Combine(_directory, "nested", "students.json");
        Student[] students =
        [
            new Student(
                "Kovalenko",
                "Olena",
                5,
                "ST-1",
                MilitaryServiceStatus.Served,
                "MB-1"),
            new FootballPlayer(
                "Player",
                "Alex",
                3,
                "ST-F",
                MilitaryServiceStatus.NotServed,
                null,
                hasBall: true),
            new Lawyer(
                "Lawyer",
                "Alex",
                3,
                "ST-L",
                MilitaryServiceStatus.MilitaryTrainingCompleted,
                null,
                isOnInternship: true)
        ];
        students[0].Graduate();
        ((FootballPlayer)students[1]).Practice();
        ((Lawyer)students[2]).Practice();
        var repository = new JsonStudentRepository(filePath);

        repository.SaveAll(students);
        IReadOnlyList<Student> restored = repository.LoadAll();

        Assert.Equal(3, restored.Count);
        Assert.True(restored[0].IsGraduated);
        Assert.Equal("MB-1", restored[0].MilitaryBookNumber);
        Assert.IsType<FootballPlayer>(restored[1]);
        Assert.True(((FootballPlayer)restored[1]).HasBall);
        Assert.Equal(1, ((FootballPlayer)restored[1]).PracticeSessions);
        Assert.IsType<Lawyer>(restored[2]);
        Assert.True(((Lawyer)restored[2]).IsOnInternship);
        Assert.Equal(1, ((Lawyer)restored[2]).PracticeSessions);
    }

    [Fact]
    public void LoadAll_ThrowsWhenFileDoesNotExist()
    {
        var repository = new JsonStudentRepository(Path.Combine(_directory, "missing.json"));

        Assert.Throws<FileNotFoundException>(repository.LoadAll);
    }

    [Theory]
    [InlineData("{")]
    [InlineData("null")]
    public void LoadAll_RejectsMalformedOrEmptyData(string json)
    {
        Directory.CreateDirectory(_directory);
        string filePath = Path.Combine(_directory, "students.json");
        File.WriteAllText(filePath, json);
        var repository = new JsonStudentRepository(filePath);

        Assert.Throws<InvalidDataException>(repository.LoadAll);
    }

    [Fact]
    public void SaveAll_RejectsNullCollection()
    {
        var repository = new JsonStudentRepository(Path.Combine(_directory, "students.json"));

        Assert.Throws<ArgumentNullException>(() => repository.SaveAll(null!));
    }

    [Fact]
    public void SaveAll_RejectsNullStudent()
    {
        var repository = new JsonStudentRepository(Path.Combine(_directory, "students.json"));
        Student[] students = [null!];

        Assert.Throws<ArgumentException>(() => repository.SaveAll(students));
    }

    [Fact]
    public void Constructor_RejectsEmptyFilePath()
    {
        Assert.Throws<ArgumentException>(() => new JsonStudentRepository(" "));
    }

    public void Dispose()
    {
        if (Directory.Exists(_directory))
        {
            Directory.Delete(_directory, recursive: true);
        }
    }
}
