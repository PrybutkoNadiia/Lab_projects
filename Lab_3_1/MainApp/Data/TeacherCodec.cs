using System.Text.Json;
using MainApp.Models;

namespace MainApp.Data;

public sealed class TeacherCodec : PersonJsonCodec<Teacher>
{
    protected override void WriteProperties(Utf8JsonWriter writer, Teacher teacher)
    {
        writer.WriteString("firstName", teacher.FirstName);
        writer.WriteString("lastName", teacher.LastName);
        writer.WriteString("employeeId", teacher.EmployeeId);
        writer.WriteString("subject", teacher.Subject);
    }

    protected override Teacher ReadPerson(JsonElement element)
    {
        string firstName = element.GetProperty("firstName").GetString()
            ?? throw new InvalidDataException("Teacher firstName cannot be null.");
        string lastName = element.GetProperty("lastName").GetString()
            ?? throw new InvalidDataException("Teacher lastName cannot be null.");
        string employeeId = element.GetProperty("employeeId").GetString()
            ?? throw new InvalidDataException("Teacher employee ID cannot be null.");
        string subject = element.GetProperty("subject").GetString()
            ?? throw new InvalidDataException("Teacher subject cannot be null.");

        return new Teacher(firstName, lastName, employeeId, subject);
    }
}
