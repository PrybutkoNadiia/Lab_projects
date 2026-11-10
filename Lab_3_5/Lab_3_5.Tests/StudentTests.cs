using Lab_3_5.BLL;

namespace Lab_3_5.Tests;

public sealed class StudentTests
{
    [Fact]
    public void Constructor_StoresTrimmedStudentData()
    {
        var student = new Student(
            " Kovalenko ",
            " Olena ",
            5,
            " ST-1 ",
            MilitaryServiceStatus.Served,
            " MB-1 ");

        Assert.Equal("Kovalenko", student.LastName);
        Assert.Equal("Olena", student.FirstName);
        Assert.Equal(5, student.Course);
        Assert.Equal("ST-1", student.StudentTicketNumber);
        Assert.Equal(MilitaryServiceStatus.Served, student.MilitaryServiceStatus);
        Assert.Equal("MB-1", student.MilitaryBookNumber);
        Assert.False(student.IsGraduated);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Constructor_RejectsMissingLastName(string? lastName)
    {
        Assert.Throws<ArgumentException>(() =>
            new Student(
                lastName!,
                "Olena",
                1,
                "ST-1",
                MilitaryServiceStatus.NotServed));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Constructor_RejectsMissingFirstName(string? firstName)
    {
        Assert.Throws<ArgumentException>(() =>
            new Student(
                "Kovalenko",
                firstName!,
                1,
                "ST-1",
                MilitaryServiceStatus.NotServed));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Constructor_RejectsMissingStudentTicket(string? ticket)
    {
        Assert.Throws<ArgumentException>(() =>
            new Student(
                "Kovalenko",
                "Olena",
                1,
                ticket!,
                MilitaryServiceStatus.NotServed));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void Constructor_RejectsCourseOutsideOneToFive(int course)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Student(
                "Kovalenko",
                "Olena",
                course,
                "ST-1",
                MilitaryServiceStatus.NotServed));
    }

    [Fact]
    public void Constructor_RejectsUnknownMilitaryStatus()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Student("Kovalenko", "Olena", 1, "ST-1", (MilitaryServiceStatus)99));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Constructor_RequiresMilitaryBookForServedStudent(string? militaryBookNumber)
    {
        Assert.Throws<ArgumentException>(() =>
            new Student(
                "Kovalenko",
                "Olena",
                1,
                "ST-1",
                MilitaryServiceStatus.Served,
                militaryBookNumber));
    }

    [Fact]
    public void Constructor_RejectsMilitaryBookForOtherStatuses()
    {
        Assert.Throws<ArgumentException>(() =>
            new Student(
                "Kovalenko",
                "Olena",
                1,
                "ST-1",
                MilitaryServiceStatus.MilitaryTrainingCompleted,
                "MB-1"));
    }

    [Fact]
    public void Graduate_MarksFifthYearStudentAsGraduated()
    {
        var student = CreateStudent(course: Student.FinalCourse);

        student.Graduate();

        Assert.True(student.IsGraduated);
    }

    [Fact]
    public void Graduate_RejectsStudentBeforeFinalYear()
    {
        var student = CreateStudent(course: Student.FinalCourse - 1);

        Assert.Throws<InvalidOperationException>(student.Graduate);
        Assert.False(student.IsGraduated);
    }

    [Fact]
    public void Graduate_RejectsSecondGraduation()
    {
        var student = CreateStudent(course: Student.FinalCourse);
        student.Graduate();

        Assert.Throws<InvalidOperationException>(student.Graduate);
    }

    [Fact]
    public void RecitePoem_ReturnsStudentAndPoem()
    {
        var student = CreateStudent();

        string recitation = student.RecitePoem("A short poem");

        Assert.Equal("Olena Kovalenko recites: A short poem", recitation);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void RecitePoem_RejectsEmptyText(string? poem)
    {
        var student = CreateStudent();

        Assert.Throws<ArgumentException>(() => student.RecitePoem(poem!));
    }

    private static Student CreateStudent(int course = 1) =>
        new("Kovalenko", "Olena", course, "ST-1", MilitaryServiceStatus.NotServed);
}
