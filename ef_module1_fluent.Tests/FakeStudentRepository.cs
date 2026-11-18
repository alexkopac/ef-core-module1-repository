using ef_module1_fluent.Models;
using ef_module1_fluent.Repositories;

namespace ef_module1_fluent.Tests;

public class FakeStudentRepository : IStudentRepository
{
    private readonly List<Student> _students = new();

    public List<Student> GetAll() => _students.ToList();

    public Student? GetById(int id) => _students.FirstOrDefault(s => s.Id == id);

    public Student? GetByEmail(string email) => _students.FirstOrDefault(s => s.Email == email);

    public void Add(Student student)
    {
        if (student.Id == 0)
        {
            student.Id = _students.Count == 0 ? 1 : _students.Max(s => s.Id) + 1;
        }
        if (student.Profile != null)
        {
            student.Profile.StudentId = student.Id;
        }
        _students.Add(student);
    }

    public void Update(Student student)
    {
        var index = _students.FindIndex(s => s.Id == student.Id);
        if (index != -1)
        {
            _students[index] = student;
        }
    }

    public void Delete(int id)
    {
        var student = GetById(id);
        if (student != null)
        {
            _students.Remove(student);
        }
    }
}