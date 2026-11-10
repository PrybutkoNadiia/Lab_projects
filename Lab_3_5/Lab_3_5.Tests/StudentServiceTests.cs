using Lab_3_5.BLL;

namespace Lab_3_5.Tests;

public sealed class StudentServiceTests
{
    [Fact]
    public void Constructor_RejectsNullRepository()
    {
        Assert.Throws<ArgumentNullException>(() => new StudentService(null!));
    }

    [Fact]
    public void GetAll_ReturnsRepositoryData()
    {
        Student[] expected = [CreateStudent("ST-1")];
        var repository = new InMemoryStudentRepository(expected);
        var service = new StudentService(repository);

        IReadOnlyList<Student> actual = service.GetAll();

        Assert.Same(expected, actual);
    }

    [Fact]
    public void GetFifthYearMilitaryStudents_IncludesOnlyEligibleStudents()
    {
        Student[] students =
        [
            CreateStudent("ST-1", course: 5, status: MilitaryServiceStatus.Served, militaryBook: "MB-1"),
            CreateStudent("ST-2", course: 5, status: MilitaryServiceStatus.MilitaryTrainingCompleted),
            CreateStudent("ST-3", course: 5),
            CreateStudent("ST-4", course: 4, status: MilitaryServiceStatus.Served, militaryBook: "MB-4"),
            GraduatedStudent("ST-5")
        ];
        var service = new StudentService(new InMemoryStudentRepository(students));

        IReadOnlyList<Student> result = service.GetFifthYearMilitaryStudents();

        Assert.Equal(["ST-1", "ST-2"], result.Select(student => student.StudentTicketNumber));
    }

    [Fact]
    public void CountFifthYearMilitaryStudents_ReturnsMatchingCount()
    {
        var repository = new InMemoryStudentRepository(
        [
            CreateStudent("ST-1", course: 5, status: MilitaryServiceStatus.Served, militaryBook: "MB-1"),
            CreateStudent("ST-2", course: 5, status: MilitaryServiceStatus.MilitaryTrainingCompleted),
            CreateStudent("ST-3")
        ]);
        var service = new StudentService(repository);

        Assert.Equal(2, service.CountFifthYearMilitaryStudents());
    }

    [Fact]
    public void Graduate_UpdatesAndSavesStudent()
    {
        Student student = CreateStudent("ST-1", course: Student.FinalCourse);
        var repository = new InMemoryStudentRepository([student]);
        var service = new StudentService(repository);

        service.Graduate(" ST-1 ");

        Assert.True(student.IsGraduated);
        Assert.Equal(1, repository.SaveCount);
    }

    [Fact]
    public void Graduate_RejectsEmptyTicket()
    {
        var service = new StudentService(new InMemoryStudentRepository([]));

        Assert.Throws<ArgumentException>(() => service.Graduate("  "));
    }

    [Fact]
    public void Graduate_RejectsUnknownTicket()
    {
        var repository = new InMemoryStudentRepository([]);
        var service = new StudentService(repository);

        Assert.Throws<KeyNotFoundException>(() => service.Graduate("ST-404"));
        Assert.Equal(0, repository.SaveCount);
    }

    [Fact]
    public void Graduate_RejectsAmbiguousTicket()
    {
        var repository = new InMemoryStudentRepository(
        [
            CreateStudent("ST-1", course: Student.FinalCourse),
            CreateStudent("ST-1", course: Student.FinalCourse)
        ]);
        var service = new StudentService(repository);

        Assert.Throws<InvalidDataException>(() => service.Graduate("ST-1"));
        Assert.Equal(0, repository.SaveCount);
    }

    [Fact]
    public void Graduate_DoesNotSaveWhenStudentCannotGraduate()
    {
        var repository = new InMemoryStudentRepository([CreateStudent("ST-1", course: 4)]);
        var service = new StudentService(repository);

        Assert.Throws<InvalidOperationException>(() => service.Graduate("ST-1"));
        Assert.Equal(0, repository.SaveCount);
    }

    private static Student CreateStudent(
        string ticket,
        int course = 1,
        MilitaryServiceStatus status = MilitaryServiceStatus.NotServed,
        string? militaryBook = null) =>
        new("Kovalenko", "Olena", course, ticket, status, militaryBook);

    private static Student GraduatedStudent(string ticket)
    {
        Student student = CreateStudent(ticket, course: Student.FinalCourse);
        student.Graduate();
        return student;
    }

    private sealed class InMemoryStudentRepository(IReadOnlyList<Student> students) : IStudentRepository
    {
        public int SaveCount { get; private set; }

        public IReadOnlyList<Student> LoadAll() => students;

        public void SaveAll(IReadOnlyList<Student> updatedStudents)
        {
            SaveCount++;
        }
    }
}
