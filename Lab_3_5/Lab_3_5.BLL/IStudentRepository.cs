namespace Lab_3_5.BLL;

public interface IStudentRepository
{
    IReadOnlyList<Student> LoadAll();

    void SaveAll(IReadOnlyList<Student> students);
}
