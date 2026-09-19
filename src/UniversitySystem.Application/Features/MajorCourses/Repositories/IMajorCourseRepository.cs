using UniversitySystem.Application.Features.MajorCourses.DTOs;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Features.MajorCourses.Repositories;

/// <summary>
/// قرارداد دسترسی به داده بخش «مدیریت دروس رشته»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند.
/// </summary>
public interface IMajorCourseRepository
{
    Task<ICollection<MajorOptionDto>> GetActiveMajorsAsync(CancellationToken cancellationToken = default);
    Task<Major?> GetMajorByIdAsync(long majorId, CancellationToken cancellationToken = default);
    Task<Curriculum?> GetActiveCurriculumForMajorAsync(long majorId, CancellationToken cancellationToken = default);
    Task<ICollection<MajorCourseDto>> GetCurriculumCoursesAsync(long curriculumId, CancellationToken cancellationToken = default);
    Task<bool> CourseExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<bool> CurriculumCourseExistsAsync(long curriculumId, long courseId, CancellationToken cancellationToken = default);
    Task<Course?> GetCourseByIdAsync(long courseId, CancellationToken cancellationToken = default);
    Task RemoveCurriculumCourseAsync(long curriculumId, long courseId, CancellationToken cancellationToken = default);
    void AddCourse(Course course);
}
