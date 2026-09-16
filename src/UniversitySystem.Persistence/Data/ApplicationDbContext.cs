using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Domain.Common;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Data;

/// <summary>
/// مرکز EF برای خواندن و ذخیره موجودیت‌ها؛ تنظیمات جدول‌ها را بارگذاری می‌کند و زمان‌های ثبت و ویرایش را به UTC می‌نویسد.
/// </summary>
public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider) : base(options)
    {
        _currentUserService = currentUserService;
        _dateTimeProvider = dateTimeProvider;
    }

    // ── Identity ─────────────────────────────────────────────────────────────────
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    // ── Academic Structure ────────────────────────────────────────────────────────
    public DbSet<Faculty> Faculties => Set<Faculty>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Major> Majors => Set<Major>();
    public DbSet<Curriculum> Curriculums => Set<Curriculum>();
    public DbSet<CurriculumCourse> CurriculumCourses => Set<CurriculumCourse>();

    // ── People ────────────────────────────────────────────────────────────────────
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Professor> Professors => Set<Professor>();

    // ── Courses ───────────────────────────────────────────────────────────────────
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<CoursePrerequisite> CoursePrerequisites => Set<CoursePrerequisite>();
    public DbSet<AcademicTerm> AcademicTerms => Set<AcademicTerm>();

    // ── Offering & Assignment ─────────────────────────────────────────────────────
    public DbSet<CourseOffering> CourseOfferings => Set<CourseOffering>();
    public DbSet<CourseOfferingSchedule> CourseOfferingSchedules => Set<CourseOfferingSchedule>();
    public DbSet<TeachingAssignment> TeachingAssignments => Set<TeachingAssignment>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    // ── Pre-Registration ──────────────────────────────────────────────────────────
    public DbSet<StudentPreRegistration> StudentPreRegistrations => Set<StudentPreRegistration>();
    public DbSet<StudentPreRegistrationItem> StudentPreRegistrationItems => Set<StudentPreRegistrationItem>();
    public DbSet<StudentCourseHistory> StudentCourseHistories => Set<StudentCourseHistory>();

    // ── Teaching Requests ─────────────────────────────────────────────────────────
    public DbSet<ProfessorTeachingRequest> ProfessorTeachingRequests => Set<ProfessorTeachingRequest>();
    public DbSet<ProfessorTeachingRequestCourse> ProfessorTeachingRequestCourses => Set<ProfessorTeachingRequestCourse>();
    public DbSet<ProfessorAvailability> ProfessorAvailabilities => Set<ProfessorAvailability>();

    // ─────────────────────────────────────────────────────────────────────────────

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Discovers and applies all IEntityTypeConfiguration<T> classes in this assembly.
        // Entity configurations must never be written inline here.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Saves all pending changes and automatically populates audit fields
    /// for any <see cref="BaseAuditableEntity"/> that was added or modified.
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        PopulateAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    // ── Auditing ─────────────────────────────────────────────────────────────────

    private void PopulateAuditFields()
    {
        var now = _dateTimeProvider.UtcNow;
        var currentUserId = _currentUserService.UserId;

        foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = currentUserId;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedAt = now;
                    entry.Entity.LastModifiedBy = currentUserId;
                    break;
            }
        }
    }
}
