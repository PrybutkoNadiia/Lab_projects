using System.Text.Json;
using MainApp.Models;

namespace MainApp.Data;

public sealed class AstronautCodec : PersonJsonCodec<Astronaut>
{
    protected override void WriteProperties(Utf8JsonWriter writer, Astronaut astronaut)
    {
        writer.WriteString("firstName", astronaut.FirstName);
        writer.WriteString("lastName", astronaut.LastName);
        writer.WriteString("astronautId", astronaut.AstronautId);
        writer.WriteString("mission", astronaut.Mission);
    }

    protected override Astronaut ReadPerson(JsonElement element)
    {
        string firstName = element.GetProperty("firstName").GetString()
            ?? throw new InvalidDataException("Astronaut firstName cannot be null.");
        string lastName = element.GetProperty("lastName").GetString()
            ?? throw new InvalidDataException("Astronaut lastName cannot be null.");
        string astronautId = element.GetProperty("astronautId").GetString()
            ?? throw new InvalidDataException("Astronaut ID cannot be null.");
        string mission = element.GetProperty("mission").GetString()
            ?? throw new InvalidDataException("Astronaut mission cannot be null.");

        return new Astronaut(firstName, lastName, astronautId, mission);
    }
}
