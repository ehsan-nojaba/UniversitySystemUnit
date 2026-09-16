using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Application.Features.StudentPreRegistration.Repositories;

namespace UniversitySystem.Application.Common.Services;

/// <summary>
/// قوانین انتخاب درس دانشجو را اعمال می‌کند: درس پاس‌شده حذف می‌شود و همه پیش‌نیازها باید پاس شده باشند؛ داده از ریپازیتوری دریافت می‌شود.
/// </summary>
public sealed class StudentCourseEligibilityService(IStudentEligibilityRepository repository) : IStudentCourseEligibilityService
{
    public async Task<ICollection<EligibleCourseDto>> GetEligibleCoursesAsync(long studentId, long academicTermId, CancellationToken cancellationToken = default)
    {
        var data = await repository.GetDataAsync(studentId, cancellationToken);
        return data.Courses.Where(c => !data.PassedCourseIds.Contains(c.CourseId)
            && c.Prerequisites.All(p => data.PassedCourseIds.Contains(p.PrerequisiteCourseId)))
            .OrderBy(c => c.RecommendedTerm).ThenBy(c => c.Code).ToList();
    }
}
