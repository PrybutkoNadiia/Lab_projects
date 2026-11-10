namespace Lab_3_5.BLL;

public sealed class StudentService
{
    private readonly IStudentRepository _repository;

    public StudentService(IStudentRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    public IReadOnlyList<Student> GetAll() => _repository.LoadAll();

    public IReadOnlyList<Student> GetFifthYearMilitaryStudents() =>
        _repository.LoadAll()
            .Where(student =>
                student.Course == Student.FinalCourse &&
                !student.IsGraduated &&
                student.MilitaryServiceStatus is
                    MilitaryServiceStatus.Served or MilitaryServiceStatus.MilitaryTrainingCompleted)
            .ToArray();

    public int CountFifthYearMilitaryStudents() =>
        GetFifthYearMilitaryStudents().Count;

    public void Graduate(string studentTicketNumber)
    {
        if (string.IsNullOrWhiteSpace(studentTicketNumber))
        {
            throw new ArgumentException(
                "The student ticket number cannot be empty.",
                nameof(studentTicketNumber));
        }

        List<Student> students = _repository.LoadAll().ToList();
        Student[] matches = students
            .Where(student => string.Equals(
                student.StudentTicketNumber,
                studentTicketNumber.Trim(),
                StringComparison.Ordinal))
            .ToArray();

        if (matches.Length == 0)
        {
            throw new KeyNotFoundException(
                $"No student has ticket number '{studentTicketNumber}'.");
        }

        if (matches.Length > 1)
        {
            throw new InvalidDataException(
                $"Student ticket number '{studentTicketNumber}' is not unique.");
        }

        matches[0].Graduate();
        _repository.SaveAll(students);
    }
}
