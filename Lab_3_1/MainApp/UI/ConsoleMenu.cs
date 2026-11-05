using MainApp.Data;
using MainApp.Interfaces;
using MainApp.Models;
using MainApp.Services;

namespace MainApp.UI;

public class ConsoleMenu
{
    private readonly PersonService _service;
    private readonly PersonFileRepository _repository;
    private readonly IPersonConsoleInput[] _personInputs;
    private readonly string _dataPath;

    public ConsoleMenu(PersonService service, PersonFileRepository repository,
        IPersonConsoleInput[] personInputs, string dataPath)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _personInputs = personInputs == null
            ? throw new ArgumentNullException(nameof(personInputs))
            : (IPersonConsoleInput[])personInputs.Clone();
        _dataPath = dataPath ?? throw new ArgumentNullException(nameof(dataPath));
    }

    public void Run()
    {
        while (true)
        {
            int[] creationOptions = PrintMenu();
            Console.Write("Choose an option: ");
            string input = Console.ReadLine()?.Trim();

            if (input == "0")
            {
                Console.WriteLine("Goodbye!");
                return;
            }

            if (!int.TryParse(input, out int option))
            {
                Console.WriteLine("Invalid option.");
                continue;
            }

            if (option == 1)
            {
                LoadFromFile();
                continue;
            }

            bool created = false;
            for (int i = 0; i < creationOptions.Length; i++)
            {
                if (option == creationOptions[i])
                {
                    _service.Add(_personInputs[i].ReadFromConsole());
                    Console.WriteLine($"{_personInputs[i].MenuName} added.");
                    created = true;
                    break;
                }
            }

            if (created)
            {
                continue;
            }

            int firstGeneralOption = 2 + _personInputs.Length;
            switch (option - firstGeneralOption)
            {
                case 0: DisplayPeople(_service.GetAll()); break;
                case 1: DisplayThirdCourseSummerStudents(); break;
                case 2: FindPerson(); break;
                case 3: DeletePerson(); break;
                case 4: DemonstrateSinging(); break;
                case 5: SaveToFile(); break;
                default: Console.WriteLine("Invalid option."); break;
            }
        }
    }

    private int[] PrintMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== Variant 1: People database ===");
        Console.WriteLine("1. Load people from file");

        int[] creationOptions = new int[_personInputs.Length];
        for (int i = 0; i < _personInputs.Length; i++)
        {
            creationOptions[i] = i + 2;
            Console.WriteLine($"{creationOptions[i]}. Add {_personInputs[i].MenuName}");
        }

        int option = 2 + _personInputs.Length;
        Console.WriteLine($"{option++}. Display all people");
        Console.WriteLine($"{option++}. Count 3rd-year students born in summer");
        Console.WriteLine($"{option++}. Find person by last name and unique ID");
        Console.WriteLine($"{option++}. Delete person by last name and unique ID");
        Console.WriteLine($"{option++}. Demonstrate singing");
        Console.WriteLine($"{option}. Save all people to file");
        Console.WriteLine("0. Exit");
        return creationOptions;
    }

    private void LoadFromFile()
    {
        Person[] people = _repository.Load(_dataPath);
        _service.SetPeople(people);
        Console.WriteLine($"Loaded {people.Length} people from '{_dataPath}'.");
    }

    private void SaveToFile()
    {
        Person[] people = _service.GetAll();
        _repository.Save(_dataPath, people);
        Console.WriteLine($"Saved {people.Length} people to '{_dataPath}'.");
    }

    private void DisplayPeople(Person[] people)
    {
        if (people.Length == 0)
        {
            Console.WriteLine("No people found.");
            return;
        }

        foreach (Person person in people)
        {
            Console.WriteLine($"Type: {person.GetType().Name}");
            Console.WriteLine($"First name: {person.FirstName}");
            Console.WriteLine($"Last name: {person.LastName}");
            Console.WriteLine($"Unique ID: {person.UniqueId}");
            foreach (PersonField field in person.GetAdditionalFields())
            {
                Console.WriteLine($"{field.Name}: {field.Value}");
            }
        }
    }

    private void DisplayThirdCourseSummerStudents()
    {
        Student[] students = _service.GetThirdCourseSummerStudents();
        DisplayPeople(students);
        Console.WriteLine($"Total: {students.Length}");
    }

    private void FindPerson()
    {
        string lastName = ReadRequired("Last name: ");
        string id = ReadRequired("Unique ID: ");
        Person person = _service.FindByLastNameAndId(lastName, id);

        if (person == null)
        {
            Console.WriteLine("Person not found.");
            return;
        }

        DisplayPeople(new[] { person });
    }

    private void DeletePerson()
    {
        string lastName = ReadRequired("Last name: ");
        string id = ReadRequired("Unique ID: ");
        Console.WriteLine(_service.RemoveByLastNameAndId(lastName, id)
            ? "Person deleted."
            : "Person not found.");
    }

    private void DemonstrateSinging()
    {
        bool found = false;
        foreach (Person person in _service.GetAll())
        {
            if (person is ISingable singer)
            {
                Console.WriteLine(singer.Sing());
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("There are no people who can sing.");
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
}
