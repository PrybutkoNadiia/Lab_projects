using System.Globalization;
using Lab_3_3.BLL;

namespace Lab_3_3.PL;

public static class Menu
{
    public static void Main()
    {
        var service = new CircleService();
        Run(service);
    }

    private static void Run(CircleService service)
    {
        CircleFileFormat format = ChooseFormat();
        string filePath = ReadText("File name", GetDefaultFileName(format));

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine($"Файл: {filePath} ({format})");
            Console.WriteLine("1. Показати кола");
            Console.WriteLine("2. Додати коло");
            Console.WriteLine("3. Видалити коло");
            Console.WriteLine("4. Створити та зберегти 6 кіл");
            Console.WriteLine("5. Перевірити серіалізацію масиву й колекції");
            Console.WriteLine("0. Вийти");
            Console.Write("Оберіть дію: ");

            string? choice = Console.ReadLine();
            if (choice is null or "0")
            {
                return;
            }

            try
            {
                switch (choice)
                {
                    case "1":
                        ShowCircles(service, filePath, format);
                        break;
                    case "2":
                        AddCircle(service, filePath, format);
                        break;
                    case "3":
                        RemoveCircle(service, filePath, format);
                        break;
                    case "4":
                        SaveSampleCircles(service, filePath, format);
                        break;
                    case "5":
                        RunSerializationDemo(service, filePath, format);
                        break;
                    default:
                        Console.WriteLine("Choose one of the listed actions.");
                        break;
                }
            }
            catch (CircleServiceException exception)
            {
                Console.WriteLine($"Помилка: {exception.Message}");
            }
        }
    }

    private static CircleFileFormat ChooseFormat()
    {
        while (true)
        {
            Console.WriteLine("Оберіть формат серіалізації:");
            Console.WriteLine("1. Бінарний");
            Console.WriteLine("2. XML");
            Console.WriteLine("3. JSON");
            Console.WriteLine("4. Власний текстовий формат");
            Console.Write("Формат: ");

            switch (Console.ReadLine())
            {
                case "1":
                    return CircleFileFormat.Binary;
                case "2":
                    return CircleFileFormat.Xml;
                case "3":
                    return CircleFileFormat.Json;
                case "4":
                    return CircleFileFormat.Custom;
                default:
                    Console.WriteLine("Введіть число від 1 до 4.");
                    break;
            }
        }
    }

    private static void ShowCircles(
        CircleService service,
        string filePath,
        CircleFileFormat format)
    {
        List<CirclePresentationModel> circles = service.GetAll(filePath, format)
            .Select(CirclePresentationModel.FromBusiness)
            .ToList();
        if (circles.Count == 0)
        {
            Console.WriteLine("У файлі ще немає кіл.");
            return;
        }

        for (int i = 0; i < circles.Count; i++)
        {
            CirclePresentationModel circle = circles[i];
            Console.WriteLine(
                $"{i + 1}. Заливка: {circle.FillColor}, контур: {circle.OutlineColor}, " +
                $"радіус: {circle.Radius}; площа: {circle.CalculateArea():F2}; " +
                $"довжина окружності: {circle.CalculateCircumference():F2}");
        }
    }

    private static void AddCircle(
        CircleService service,
        string filePath,
        CircleFileFormat format)
    {
        string fillColor = ReadText("Колір заповнення", string.Empty);
        string outlineColor = ReadText("Колір контуру", string.Empty);
        double radius = ReadRadius();

        var circle = new CirclePresentationModel(fillColor, outlineColor, radius);
        service.Add(filePath, format, circle.ToBusinessEntity());
        Console.WriteLine("Коло збережено.");
    }

    private static void RemoveCircle(
        CircleService service,
        string filePath,
        CircleFileFormat format)
    {
        List<Circle> circles = service.GetAll(filePath, format);
        if (circles.Count == 0)
        {
            Console.WriteLine("Немає кіл для видалення.");
            return;
        }

        ShowCircles(service, filePath, format);
        int number = ReadInteger("Номер кола: ");
        service.RemoveAt(filePath, format, number - 1);
        Console.WriteLine("Коло видалено.");
    }

    private static void SaveSampleCircles(
        CircleService service,
        string filePath,
        CircleFileFormat format)
    {
        List<Circle> circles = CircleService.CreateSampleCircles();
        service.SaveAll(filePath, format, circles);
        Console.WriteLine($"Збережено кіл: {circles.Count}.");
    }

    private static void RunSerializationDemo(
        CircleService service,
        string filePath,
        CircleFileFormat format)
    {
        SerializationDemoResult result = service.RunSerializationDemo(filePath, format);
        Console.WriteLine($"Серіалізовано й відновлено кіл: {result.CircleCount}.");
        Console.WriteLine($"Масив: {(result.ArrayValuesMatch ? "дані збігаються" : "дані не збігаються")}.");
        Console.WriteLine($"Колекція: {(result.CollectionValuesMatch ? "дані збігаються" : "дані не збігаються")}.");
    }

    private static double ReadRadius()
    {
        while (true)
        {
            Console.Write("Радіус: ");
            string? input = Console.ReadLine();
            if (double.TryParse(input, NumberStyles.Float, CultureInfo.CurrentCulture, out double radius) ||
                double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out radius))
            {
                return radius;
            }

            Console.WriteLine("Введіть число, наприклад 2,5.");
        }
    }

    private static int ReadInteger(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int number))
            {
                return number;
            }

            Console.WriteLine("Введіть ціле число.");
        }
    }

    private static string ReadText(string prompt, string defaultValue)
    {
        Console.Write($"{prompt}{(defaultValue.Length > 0 ? $" [{defaultValue}]" : string.Empty)}: ");
        string? value = Console.ReadLine();
        return string.IsNullOrWhiteSpace(value) ? defaultValue : value.Trim();
    }

    private static string GetDefaultFileName(CircleFileFormat format) =>
        format switch
        {
            CircleFileFormat.Binary => "circles.bin",
            CircleFileFormat.Xml => "circles.xml",
            CircleFileFormat.Json => "circles.json",
            CircleFileFormat.Custom => "circles.txt",
            _ => throw new CircleServiceException("The selected file format is not supported.")
        };
}
