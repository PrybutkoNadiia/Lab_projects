using System.Text.Json;
using MainApp.Models;

namespace MainApp.Data;

public sealed class StudentCodec : PersonJsonCodec<Student>
{
    protected override void WriteProperties(Utf8JsonWriter writer, Student student)
    {
        writer.WriteString("firstName", student.FirstName);
        writer.WriteString("lastName", student.LastName);
        writer.WriteNumber("course", student.Course);
        writer.WriteString("studentCardNumber", student.StudentCardNumber);
        writer.WriteString("birthDate", student.BirthDate.ToString("dd-MM yyyy"));
    }

    protected override Student ReadPerson(JsonElement element)
    {
        string firstName = element.GetProperty("firstName").GetString()
            ?? throw new InvalidDataException("Student firstName cannot be null.");
        string lastName = element.GetProperty("lastName").GetString()
            ?? throw new InvalidDataException("Student lastName cannot be null.");
        int course = element.GetProperty("course").GetInt32();
        string card = element.GetProperty("studentCardNumber").GetString()
            ?? throw new InvalidDataException("Student card number cannot be null.");
        string birthDateText = element.GetProperty("birthDate").GetString()
            ?? throw new InvalidDataException("Student birth date cannot be null.");
        if (!DateTime.TryParseExact(birthDateText, "dd-MM yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime birthDate))
        {
            throw new InvalidDataException($"Invalid student birth date '{birthDateText}'. Expected dd-MM yyyy.");
        }

        return new Student(firstName, lastName, course, card, birthDate);
    }
}
