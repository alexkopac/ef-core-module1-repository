using ef_module1_fluent.Controllers;
using ef_module1_fluent.Models;

namespace ef_module1_fluent.Tests;

public class StudentsControllerTests
{
    [Fact]
    public void RegisterStudent_ValidData_AddsStudentWithProfile()
    {
        // Arrange
        var fakeRepo = new FakeStudentRepository();
        var controller = new StudentsController(fakeRepo);

        // Act
        var result = controller.RegisterStudent(
            "Олександр Копач",
            "oleksandr@example.com",
            "ST-2026-001",
            "https://github.com/alexkopac");

        // Assert
        Assert.Equal(1, result.Id);
        Assert.Equal("Олександр Копач", result.FullName);
        Assert.NotNull(result.Profile);
        Assert.Equal("ST-2026-001", result.Profile.CardNumber);
        Assert.Single(controller.GetAllStudents());
    }

    [Fact]
    public void RegisterStudent_DuplicateEmail_ThrowsInvalidOperationException()
    {
        // Arrange
        var fakeRepo = new FakeStudentRepository();
        fakeRepo.Add(new Student { Id = 1, FullName = "Ірина Мельник", Email = "iryna@example.com" });
        var controller = new StudentsController(fakeRepo);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            controller.RegisterStudent("Інший Студент", "iryna@example.com", "ST-2026-002", "https://github.com/test"));
    }

    [Fact]
    public void GetStudentDetails_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var fakeRepo = new FakeStudentRepository();
        var controller = new StudentsController(fakeRepo);

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => controller.GetStudentDetails(999));
    }

    [Fact]
    public void UpdateStudentEmail_ExistingStudent_UpdatesEmailSuccessfully()
    {
        // Arrange
        var fakeRepo = new FakeStudentRepository();
        fakeRepo.Add(new Student { Id = 1, FullName = "Дмитро Коваленко", Email = "old@example.com" });
        var controller = new StudentsController(fakeRepo);

        // Act
        controller.UpdateStudentEmail(1, "new@example.com");

        // Assert
        var updated = controller.GetStudentDetails(1);
        Assert.Equal("new@example.com", updated.Email);
    }

    [Fact]
    public void RemoveStudent_ExistingStudent_DeletesFromRepository()
    {
        // Arrange
        var fakeRepo = new FakeStudentRepository();
        fakeRepo.Add(new Student { Id = 1, FullName = "Олена Бондаренко", Email = "olena@example.com" });
        var controller = new StudentsController(fakeRepo);

        // Act
        controller.RemoveStudent(1);

        // Assert
        Assert.Empty(controller.GetAllStudents());
    }
}