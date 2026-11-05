using System.Text.Json;
using System.Text;
using MainApp.Models;

namespace MainApp.Data;

public abstract class PersonJsonCodec<TPerson> : IPersonCodec where TPerson : Person
{
    public string TypeName => typeof(TPerson).Name;

    public bool CanSerialize(Person person) => person.GetType() == typeof(TPerson);

    public string Serialize(Person person)
    {
        using MemoryStream stream = new MemoryStream();
        using (Utf8JsonWriter writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true }))
        {
            writer.WriteStartObject();
            WriteProperties(writer, (TPerson)person);
            writer.WriteEndObject();
        }

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    public Person Deserialize(string json)
    {
        using JsonDocument document = JsonDocument.Parse(json);
        return ReadPerson(document.RootElement);
    }

    protected abstract void WriteProperties(Utf8JsonWriter writer, TPerson person);
    protected abstract TPerson ReadPerson(JsonElement element);
}
