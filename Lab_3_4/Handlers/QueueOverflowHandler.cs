using Lab_3_4.Collections;
using Lab_3_4.Events;

namespace Lab_3_4.Handlers;

public sealed class QueueOverflowHandler<T>
{
    public void OnQueueOverflow(object? sender, QueueOverflowEventArgs<T> eventArgs)
    {
        string source = sender is BoundedQueue<T> ? "обмежена черга" : "невідомий об’єкт";

        Console.WriteLine(
            $"Подія переповнення від об’єкта «{source}»: черга заповнена " +
            $"({eventArgs.Count}/{eventArgs.Capacity}); " +
            $"елемент «{eventArgs.Item}» не додано.");
    }
}
