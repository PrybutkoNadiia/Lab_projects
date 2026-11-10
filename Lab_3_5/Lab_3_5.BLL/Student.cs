using System.Text.Json.Serialization;

namespace Lab_3_5.BLL;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(FootballPlayer), "footballPlayer")]
[JsonDerivedType(typeof(Lawyer), "lawyer")]
public class Student
{
    public const int FinalCourse = 5;

    public string LastName { get; }
    public string FirstName { get; }
    public int Course { get; }
    public string StudentTicketNumber { get; }
    public MilitaryServiceStatus MilitaryServiceStatus { get; }
    public string? MilitaryBookNumber { get; }

    [JsonInclude]
    public bool IsGraduated { get; private set; }

    public Student(
        string lastName,
        string firstName,
        int course,
        string studentTicketNumber,
        MilitaryServiceStatus militaryServiceStatus,
        string? militaryBookNumber = null)
    {
        LastName = RequiredText(lastName, nameof(lastName));
        FirstName = RequiredText(firstName, nameof(firstName));
        StudentTicketNumber = RequiredText(studentTicketNumber, nameof(studentTicketNumber));

        if (course is < 1 or > FinalCourse)
        {
            throw new ArgumentOutOfRangeException(
                nameof(course),
                $"Course must be between 1 and {FinalCourse}.");
        }

        if (!Enum.IsDefined(militaryServiceStatus))
        {
            throw new ArgumentOutOfRangeException(nameof(militaryServiceStatus));
        }

        if (militaryServiceStatus == MilitaryServiceStatus.Served)
        {
            MilitaryBookNumber = RequiredText(militaryBookNumber, nameof(militaryBookNumber));
        }
        else if (militaryBookNumber is not null)
        {
            throw new ArgumentException(
                "A military book number is only required for students who served.",
                nameof(militaryBookNumber));
        }

        Course = course;
        MilitaryServiceStatus = militaryServiceStatus;
    }

    public void Graduate()
    {
        if (IsGraduated)
        {
            throw new InvalidOperationException("The student has already graduated.");
        }

        if (Course != FinalCourse)
        {
            throw new InvalidOperationException(
                $"Only a {FinalCourse}th-year student can graduate.");
        }

        IsGraduated = true;
    }

    public string RecitePoem(string poem)
    {
        string text = RequiredText(poem, nameof(poem));
        return $"{FirstName} {LastName} recites: {text}";
    }

    private static string RequiredText(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("The value cannot be empty.", parameterName);
        }

        return value.Trim();
    }
}
