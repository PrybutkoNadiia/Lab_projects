using System.Text.Json.Serialization;

namespace Lab_3_5.BLL;

public sealed class Lawyer : Student
{
    public bool IsOnInternship { get; set; }

    [JsonInclude]
    public int PracticeSessions { get; private set; }

    public Lawyer(
        string lastName,
        string firstName,
        int course,
        string studentTicketNumber,
        MilitaryServiceStatus militaryServiceStatus,
        string? militaryBookNumber,
        bool isOnInternship)
        : base(
            lastName,
            firstName,
            course,
            studentTicketNumber,
            militaryServiceStatus,
            militaryBookNumber)
    {
        IsOnInternship = isOnInternship;
    }

    public bool Practice()
    {
        if (!IsOnInternship)
        {
            return false;
        }

        PracticeSessions++;
        return true;
    }
}
