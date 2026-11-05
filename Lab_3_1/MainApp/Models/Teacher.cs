using MainApp.Interfaces;

namespace MainApp.Models;

public class Teacher : Person, ISingable
{
    public string EmployeeId { get; set; }
    public string Subject { get; set; }

    public override string UniqueId => EmployeeId;

    public Teacher(string firstName, string lastName, string employeeId, string subject)
        : base(firstName, lastName)
    {
        EmployeeId = employeeId;
        Subject = subject;
    }

    public override PersonField[] GetAdditionalFields() =>
        new[]
        {
            new PersonField("Subject", Subject)
        };

    public string Sing() => $"{GetFullName()} (teacher) sings a song.";
}
