using System.Text.RegularExpressions;
using MainApp.Models;

namespace MainApp.UI;

public sealed class StudentConsoleInput : IPersonConsoleInput
{
    public string MenuName => "student";

    public Person ReadFromConsole()
    {
        string firstName = ConsolePrompts.ReadName("First name: ");
        string lastName = ConsolePrompts.ReadName("Last name: ");
        int course = ConsolePrompts.ReadCourse();
        string card = ConsolePrompts.ReadMatching(
            "Student card (2 uppercase letters + 6 digits): ",
            @"^[A-Z]{2}\d{6}$");
        DateTime birthDate = ConsolePrompts.ReadBirthDate("Birth date (dd-MM yyyy): ");

        return new Student(firstName, lastName, course, card, birthDate);
    }
}

public sealed class TeacherConsoleInput : IPersonConsoleInput
{
    public string MenuName => "teacher";

    public Person ReadFromConsole()
    {
        string firstName = ConsolePrompts.ReadName("First name: ");
        string lastName = ConsolePrompts.ReadName("Last name: ");
        string id = ConsolePrompts.ReadMatching("Employee ID (T + 6 digits): ", @"^T\d{6}$");
        string subject = ConsolePrompts.ReadName("Subject: ");
        return new Teacher(firstName, lastName, id, subject);
    }
}

public sealed class AstronautConsoleInput : IPersonConsoleInput
{
    public string MenuName => "astronaut";

    public Person ReadFromConsole()
    {
        string firstName = ConsolePrompts.ReadName("First name: ");
        string lastName = ConsolePrompts.ReadName("Last name: ");
        string id = ConsolePrompts.ReadMatching("Astronaut ID (A + 6 digits): ", @"^A\d{6}$");
        string mission = ConsolePrompts.ReadName("Mission: ");
        return new Astronaut(firstName, lastName, id, mission);
    }
}

internal static class ConsolePrompts
{
    private static readonly Regex NamePattern = new Regex(@"^[\p{L}][\p{L}' -]*$");

    public static string ReadName(string prompt) => ReadMatching(prompt, NamePattern);

    public static string ReadMatching(string prompt, string pattern) =>
        ReadMatching(prompt, new Regex(pattern));

    private static string ReadMatching(string prompt, Regex pattern)
    {
        while (true)
        {
            string value = ReadRequired(prompt);
            if (pattern.IsMatch(value))
            {
                return value;
            }

            Console.WriteLine("Value has an invalid format.");
        }
    }

    private static string ReadRequired(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string value = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            Console.WriteLine("Value cannot be empty.");
        }
    }

    public static int ReadCourse()
    {
        while (true)
        {
            Console.Write("Course (1-6): ");
            if (int.TryParse(Console.ReadLine(), out int course) && course >= 1 && course <= 6)
            {
                return course;
            }

            Console.WriteLine("Enter a whole number from 1 to 6.");
        }
    }

    public static DateTime ReadBirthDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string value = Console.ReadLine()?.Trim();
            if (DateTime.TryParseExact(value, "dd-MM yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime birthDate))
            {
                return birthDate;
            }

            Console.WriteLine("Enter a valid date in dd-MM yyyy format.");
        }
    }
}
