using System.Text;
using FileIO.Lib;
using MainApp.Models;

namespace MainApp.Data;

public sealed class PersonFileRepository
{
    private readonly ITextFileStore _fileStore;
    private readonly IPersonCodec[] _codecs;

    public PersonFileRepository(ITextFileStore fileStore, IPersonCodec[] codecs)
    {
        _fileStore = fileStore ?? throw new ArgumentNullException(nameof(fileStore));
        _codecs = codecs == null ? throw new ArgumentNullException(nameof(codecs)) : (IPersonCodec[])codecs.Clone();
    }

    public Person[] Load(string path)
    {
        string content = _fileStore.ReadAll(path);
        if (string.IsNullOrWhiteSpace(content))
        {
            return Array.Empty<Person>();
        }

        string normalized = content.Replace("\r\n", "\n");
        string[] records = normalized.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
        Person[] people = Array.Empty<Person>();

        foreach (string record in records)
        {
            int separator = record.IndexOf('\n');
            if (separator <= 0 || separator == record.Length - 1)
            {
                throw new InvalidDataException("A person record must contain a type/name line and a JSON object.");
            }

            string header = record.Substring(0, separator).Trim();
            int nameSeparator = header.IndexOf(' ');
            string typeName = nameSeparator < 0 ? header : header.Substring(0, nameSeparator);
            string json = record.Substring(separator + 1).Trim();
            IPersonCodec codec = FindCodec(typeName);
            Array.Resize(ref people, people.Length + 1);
            people[^1] = codec.Deserialize(json);
        }

        return people;
    }

    public void Save(string path, Person[] people)
    {
        StringBuilder content = new StringBuilder();
        foreach (Person person in people)
        {
            IPersonCodec codec = FindCodec(person);
            content.Append(codec.TypeName)
                .Append(' ')
                .Append(person.GetFullName().Replace(" ", string.Empty))
                .AppendLine()
                .AppendLine(codec.Serialize(person))
                .AppendLine();
        }

        _fileStore.WriteAll(path, content.ToString());
    }

    private IPersonCodec FindCodec(string typeName)
    {
        foreach (IPersonCodec codec in _codecs)
        {
            if (string.Equals(codec.TypeName, typeName, StringComparison.Ordinal))
            {
                return codec;
            }
        }

        throw new InvalidDataException($"No person codec is registered for type '{typeName}'.");
    }

    private IPersonCodec FindCodec(Person person)
    {
        foreach (IPersonCodec codec in _codecs)
        {
            if (codec.CanSerialize(person))
            {
                return codec;
            }
        }

        throw new InvalidDataException($"No person codec is registered for '{person.GetType().Name}'.");
    }
}
