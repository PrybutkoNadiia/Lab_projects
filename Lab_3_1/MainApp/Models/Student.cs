namespace MainApp.Models;

public class Student : Person
{
    public int Course { get; set; }
    public string StudentCardNumber { get; set; }
    public DateTime BirthDate { get; set; }

    public override string UniqueId => StudentCardNumber;

    public Student(string firstName, string lastName, int course, string studentCardNumber,
        DateTime birthDate) : base(firstName, lastName)
    {
        Course = course;
        StudentCardNumber = studentCardNumber;
        BirthDate = birthDate;
    }

    public override PersonField[] GetAdditionalFields() =>
        new[]
        {
            new PersonField("Course", Course.ToString()),
            new PersonField("Birth date", BirthDate.ToString("dd-MM yyyy"))
        };

    public void Study() => Course++;
}
