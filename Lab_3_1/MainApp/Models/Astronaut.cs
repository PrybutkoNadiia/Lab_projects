using MainApp.Interfaces;

namespace MainApp.Models;

public class Astronaut : Person, ISingable
{
    public string AstronautId { get; set; }
    public string Mission { get; set; }

    public override string UniqueId => AstronautId;

    public Astronaut(string firstName, string lastName, string astronautId, string mission)
        : base(firstName, lastName)
    {
        AstronautId = astronautId;
        Mission = mission;
    }

    public override PersonField[] GetAdditionalFields() =>
        new[]
        {
            new PersonField("Mission", Mission)
        };

    public string Sing() => $"{GetFullName()} (astronaut) sings a song.";
}
