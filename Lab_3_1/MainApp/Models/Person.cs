namespace MainApp.Models;

public abstract class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public abstract string UniqueId { get; }

    protected Person(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string GetFullName() => $"{FirstName} {LastName}";

    public virtual PersonField[] GetAdditionalFields() => Array.Empty<PersonField>();
}
