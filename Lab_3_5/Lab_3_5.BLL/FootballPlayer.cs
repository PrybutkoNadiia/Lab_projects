using System.Text.Json.Serialization;

namespace Lab_3_5.BLL;

public sealed class FootballPlayer : Student
{
    public bool HasBall { get; set; }

    [JsonInclude]
    public int PracticeSessions { get; private set; }

    public FootballPlayer(
        string lastName,
        string firstName,
        int course,
        string studentTicketNumber,
        MilitaryServiceStatus militaryServiceStatus,
        string? militaryBookNumber,
        bool hasBall)
        : base(
            lastName,
            firstName,
            course,
            studentTicketNumber,
            militaryServiceStatus,
            militaryBookNumber)
    {
        HasBall = hasBall;
    }

    public bool Practice()
    {
        if (!HasBall)
        {
            return false;
        }

        PracticeSessions++;
        return true;
    }
}

