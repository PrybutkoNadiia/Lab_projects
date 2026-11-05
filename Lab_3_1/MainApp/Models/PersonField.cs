namespace MainApp.Models;

public sealed class PersonField
{
    public string Name { get; }
    public string Value { get; }

    public PersonField(string name, string value)
    {
        Name = name;
        Value = value;
    }
}
