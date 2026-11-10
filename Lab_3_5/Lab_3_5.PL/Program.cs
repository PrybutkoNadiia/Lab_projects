using Lab_3_5.BLL;
using Lab_3_5.DAL;

string dataFile = Path.GetFullPath(Path.Combine("data", "students.json"));
var service = new StudentService(new JsonStudentRepository(dataFile));

Console.WriteLine("Student registry — Lab 3.5, variant 4");
Console.WriteLine($"Data file: {dataFile}");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1. Show all students");
    Console.WriteLine("2. Show fifth-year students with military service/training");
    Console.WriteLine("3. Graduate a fifth-year student");
    Console.WriteLine("4. Recite a poem");
    Console.WriteLine("5. Football player practice");
    Console.WriteLine("6. Lawyer practice");
    Console.WriteLine("0. Exit");
    Console.Write("Select an option: ");

    string? choice = Console.ReadLine();
    if (choice == "0")
    {
        break;
    }

    try
    {
        switch (choice)
        {
            case "1":
                ShowStudents(service.GetAll());
                break;
            case "2":
                IReadOnlyList<Student> eligibleStudents = service.GetFifthYearMilitaryStudents();
                ShowStudents(eligibleStudents);
                Console.WriteLine($"Count: {eligibleStudents.Count}");
                break;
            case "3":
                Console.Write("Student ticket number: ");
                service.Graduate(Console.ReadLine() ?? string.Empty);
                Console.WriteLine("The student has graduated.");
                break;
            case "4":
                RecitePoem(service);
                break;
            case "5":
                PracticeFootball();
                break;
            case "6":
                PracticeLaw();
                break;
            default:
                Console.WriteLine("Choose one of the listed options.");
                break;
        }
    }
    catch (ArgumentException exception)
    {
        Console.WriteLine($"Invalid input: {exception.Message}");
    }
    catch (InvalidOperationException exception)
    {
        Console.WriteLine($"Operation could not be completed: {exception.Message}");
    }
    catch (KeyNotFoundException exception)
    {
        Console.WriteLine(exception.Message);
    }
    catch (InvalidDataException exception)
    {
        Console.WriteLine($"Invalid student data: {exception.Message}");
    }
    catch (IOException exception)
    {
        Console.WriteLine($"File operation failed: {exception.Message}");
    }
}

static void ShowStudents(IEnumerable<Student> students)
{
    foreach (Student student in students)
    {
        string militaryInfo = student.MilitaryServiceStatus == MilitaryServiceStatus.Served
            ? $", military book: {student.MilitaryBookNumber}"
            : string.Empty;
        string graduationInfo = student.IsGraduated ? ", graduated" : string.Empty;
        Console.WriteLine(
            $"{student.StudentTicketNumber}: {student.LastName} {student.FirstName}, " +
            $"course {student.Course}, {student.MilitaryServiceStatus}{militaryInfo}{graduationInfo}");
    }
}

static void RecitePoem(StudentService studentService)
{
    Student? student = studentService.GetAll().FirstOrDefault();
    if (student is null)
    {
        Console.WriteLine("Add a student before using this operation.");
        return;
    }

    Console.Write("Enter a poem: ");
    Console.WriteLine(student.RecitePoem(Console.ReadLine() ?? string.Empty));
}

static void PracticeFootball()
{
    Console.Write("Is a ball available? (y/n): ");
    bool hasBall = ReadYesNo();
    var player = new FootballPlayer(
        "Player",
        "Alex",
        3,
        "DEMO-F",
        MilitaryServiceStatus.NotServed,
        null,
        hasBall);
    Console.WriteLine(player.Practice()
        ? "Football practice completed."
        : "Practice is unavailable without a ball.");
}

static void PracticeLaw()
{
    Console.Write("Is the lawyer on an internship? (y/n): ");
    bool isOnInternship = ReadYesNo();
    var lawyer = new Lawyer(
        "Lawyer",
        "Alex",
        3,
        "DEMO-L",
        MilitaryServiceStatus.NotServed,
        null,
        isOnInternship);
    Console.WriteLine(lawyer.Practice()
        ? "Legal practice completed."
        : "Practice is unavailable without an internship.");
}

static bool ReadYesNo()
{
    while (true)
    {
        string? answer = Console.ReadLine();
        if (string.Equals(answer, "y", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (string.Equals(answer, "n", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        Console.Write("Enter y or n: ");
    }
}
