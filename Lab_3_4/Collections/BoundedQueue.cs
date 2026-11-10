using Lab_3_4.Events;

namespace Lab_3_4.Collections;

public sealed class BoundedQueue<T>
{
    private readonly Queue<T> _items = new();

    public BoundedQueue(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(capacity),
                "Місткість черги має бути більшою за нуль.");
        }

        Capacity = capacity;
    }

    public event EventHandler<QueueOverflowEventArgs<T>>? Overflowed;

    public int Capacity { get; }

    public int Count => _items.Count;

    public bool Enqueue(T item)
    {
        if (_items.Count == Capacity)
        {
            Overflowed?.Invoke(
                this,
                new QueueOverflowEventArgs<T>(item, _items.Count, Capacity));

            return false;
        }

        _items.Enqueue(item);
        return true;
    }

    public bool TryDequeue(out T? item)
    {
        return _items.TryDequeue(out item);
    }
}
