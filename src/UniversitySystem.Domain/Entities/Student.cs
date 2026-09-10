using UniversitySystem.Domain.Common;

namespace UniversitySystem.Domain.Entities;

public class Student : BaseAuditableEntity
{
    public long UserId { get; private set; }
    public string StudentNumber { get; private set; } = default!;
    public long MajorId { get; private set; }
    public int EntryYear { get; private set; }

    public User User { get; private set; } = default!;
    public Major Major { get; private set; } = default!;

    private readonly List<StudentCourseHistory> _courseHistory = new();
    public IReadOnlyCollection<StudentCourseHistory> CourseHistory => _courseHistory.AsReadOnly();

    private readonly List<StudentPreRegistration> _preRegistrations = new();
    public IReadOnlyCollection<StudentPreRegistration> PreRegistrations => _preRegistrations.AsReadOnly();

    private readonly List<Enrollment> _enrollments = new();
    public IReadOnlyCollection<Enrollment> Enrollments => _enrollments.AsReadOnly();

    private Student() { }

    public Student(long userId, string studentNumber, long majorId, int entryYear)
    {
        UserId = userId;
        StudentNumber = studentNumber.Trim();
        MajorId = majorId;
        EntryYear = entryYear;
    }
}
