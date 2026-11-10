using Lab_3_4.Collections;
using Lab_3_4.Handlers;

namespace Lab_3_4;

internal static class Program
{
    private static void Main()
    {
        DemonstrateCharacterSorting();
        DemonstrateBoundedQueue();
    }

    private static void DemonstrateCharacterSorting()
    {
        char[] characters = ['д', 'а', 'в', 'б', 'а'];

        Func<char[], char[]> lambdaSorter = values =>
            values.OrderByDescending(character => character).ToArray();

        Func<char[], char[]> anonymousMethodSorter = delegate (char[] values)
        {
            return values.OrderByDescending(character => character).ToArray();
        };

        Console.WriteLine("Сортування масиву символів за спаданням:");
        Console.WriteLine($"Початковий масив: {new string(characters)}");
        Console.WriteLine($"Лямбда-вираз:     {new string(lambdaSorter(characters))}");
        Console.WriteLine($"Анонімний метод:  {new string(anonymousMethodSorter(characters))}");
        Console.WriteLine();
    }

    private static void DemonstrateBoundedQueue()
    {
        var queue = new BoundedQueue<char>(capacity: 3);
        var overflowHandler = new QueueOverflowHandler<char>();
        queue.Overflowed += overflowHandler.OnQueueOverflow;

        Console.WriteLine("Демонстрація обмеженої черги:");
        queue.Enqueue('А');
        queue.Enqueue('Б');
        queue.Enqueue('В');

        // Спроба додати четвертий елемент викликає подію переповнення.
        queue.Enqueue('Г');

        queue.TryDequeue(out char removedCharacter);
        Console.WriteLine($"Видалено з черги: {removedCharacter}");

        queue.Enqueue('Г');
        Console.WriteLine("Елементи черги після видалення та повторного додавання:");

        while (queue.TryDequeue(out char character))
        {
            Console.WriteLine($"  {character}");
        }
    }
}
