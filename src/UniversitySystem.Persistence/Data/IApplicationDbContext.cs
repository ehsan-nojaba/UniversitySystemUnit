using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Data;

/// <summary>
/// قرارداد داخلی Persistence برای مجموعه‌های دیتابیس و آماده‌سازی داده تست؛ Application از ریپازیتوری‌های اختصاصی استفاده می‌کند.
/// </summary>
public interface IApplicationDbContext
{
    // ── Identity ──────────────────────────────────────────────────────────────────
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<UserRole> UserRoles { get; }

    // ── Academic Structure ─────────────────────────────────────────────────────────
    DbSet<Faculty> Faculties { get; }
    DbSet<Department> Departments { get; }
    DbSet<Major> Majors { get; }
    DbSet<Curriculum> Curriculums { get; }
    DbSet<CurriculumCourse> CurriculumCourses { get; }

    // ── People ─────────────────────────────────────────────────────────────────────
    DbSet<Student> Students { get; }
    DbSet<Professor> Professors { get; }

    // ── Courses ────────────────────────────────────────────────────────────────────
    DbSet<Course> Courses { get; }
    DbSet<CoursePrerequisite> CoursePrerequisites { get; }
    DbSet<AcademicTerm> AcademicTerms { get; }

    // ── Offering & Assignment ──────────────────────────────────────────────────────
    DbSet<CourseOffering> CourseOfferings { get; }
    DbSet<CourseOfferingSchedule> CourseOfferingSchedules { get; }
    DbSet<TeachingAssignment> TeachingAssignments { get; }
    DbSet<Enrollment> Enrollments { get; }

    // ── Pre-Registration ───────────────────────────────────────────────────────────
    DbSet<StudentPreRegistration> StudentPreRegistrations { get; }
    DbSet<StudentPreRegistrationItem> StudentPreRegistrationItems { get; }
    DbSet<StudentCourseHistory> StudentCourseHistories { get; }

    // ── Teaching Requests ──────────────────────────────────────────────────────────
    DbSet<ProfessorTeachingRequest> ProfessorTeachingRequests { get; }
    DbSet<ProfessorTeachingRequestCourse> ProfessorTeachingRequestCourses { get; }
    DbSet<ProfessorAvailability> ProfessorAvailabilities { get; }

    // ─────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Saves all pending changes asynchronously.
    /// Audit fields are populated automatically by <c>ApplicationDbContext</c>.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
