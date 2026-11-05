using MainApp.Models;

namespace MainApp.Services;

public class PersonService
{
    private Person[] _people = Array.Empty<Person>();

    public Person[] GetAll() => (Person[])_people.Clone();

    public void SetPeople(Person[] people)
    {
        _people = people == null ? Array.Empty<Person>() : (Person[])people.Clone();
    }

    public void Add(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);
        Array.Resize(ref _people, _people.Length + 1);
        _people[^1] = person;
    }

    public Person FindByLastNameAndId(string lastName, string uniqueId)
    {
        foreach (Person person in _people)
        {
            if (string.Equals(person.LastName, lastName, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(person.UniqueId, uniqueId, StringComparison.OrdinalIgnoreCase))
            {
                return person;
            }
        }

        return null;
    }

    public bool RemoveByLastNameAndId(string lastName, string uniqueId)
    {
        for (int i = 0; i < _people.Length; i++)
        {
            if (string.Equals(_people[i].LastName, lastName, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(_people[i].UniqueId, uniqueId, StringComparison.OrdinalIgnoreCase))
            {
                Person[] remaining = new Person[_people.Length - 1];
                Array.Copy(_people, 0, remaining, 0, i);
                Array.Copy(_people, i + 1, remaining, i, _people.Length - i - 1);
                _people = remaining;
                return true;
            }
        }

        return false;
    }

    public Student[] GetThirdCourseSummerStudents()
    {
        int count = 0;
        foreach (Person person in _people)
        {
            if (person is Student student && student.Course == 3 &&
                student.BirthDate.Month >= 6 && student.BirthDate.Month <= 8)
            {
                count++;
            }
        }

        Student[] result = new Student[count];
        int index = 0;
        foreach (Person person in _people)
        {
            if (person is Student student && student.Course == 3 &&
                student.BirthDate.Month >= 6 && student.BirthDate.Month <= 8)
            {
                result[index++] = student;
            }
        }

        return result;
    }
}
