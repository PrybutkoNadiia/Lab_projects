namespace Lab_3_3.BLL;

public class CircleServiceException : Exception
{
    public CircleServiceException(string message) : base(message)
    {
    }

    public CircleServiceException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
