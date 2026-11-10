namespace Lab_3_4.Events;

public sealed class QueueOverflowEventArgs<T> : EventArgs
{
    public QueueOverflowEventArgs(T item, int count, int capacity)
    {
        Item = item;
        Count = count;
        Capacity = capacity;
    }

    public T Item { get; }

    public int Count { get; }

    public int Capacity { get; }
}
