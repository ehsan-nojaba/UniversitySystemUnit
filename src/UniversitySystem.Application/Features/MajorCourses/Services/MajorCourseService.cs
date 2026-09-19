using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Features.MajorCourses.DTOs;
using UniversitySystem.Application.Features.MajorCurricula;
using MajorCourseDto = UniversitySystem.Application.Features.MajorCourses.DTOs.MajorCourseDto;
using MajorOptionDto = UniversitySystem.Application.Features.MajorCourses.DTOs.MajorOptionDto;

namespace UniversitySystem.Application.Features.MajorCourses.Services;

/// <summary>
/// مسیرهای قبلی مدیریت درس را به منطق واحد چارت متصل می‌کند؛ اعتبارسنجی، تراکنش و حفاظت سوابق در MajorCurriculumService اجرا می‌شوند.
/// </summary>
public sealed class MajorCourseService(MajorCurriculumService curriculumService) : IMajorCourseService
{
    public async Task<ICollection<MajorOptionDto>> GetMajorsAsync(CancellationToken cancellationToken = default)
        => (await curriculumService.GetMajorsAsync(cancellationToken)).Where(m => m.IsActive).Select(m => new MajorOptionDto(m.Id, m.Code, m.Title)).ToList();

    public async Task<ICollection<MajorCourseDto>> GetCoursesForMajorAsync(long majorId, CancellationToken cancellationToken = default)
        => (await curriculumService.GetAsync(majorId, cancellationToken)).Courses.Select(Map).ToList();

    public async Task<MajorCourseDto> CreateCourseForMajorAsync(long majorId, string code, string title, int credits, int recommendedTerm, bool isRequired, CancellationToken cancellationToken = default)
    {
        var curriculum = await curriculumService.CreateCourseAsync(majorId, new(code, title, credits, recommendedTerm, isRequired), cancellationToken);
        return Map(curriculum.Courses.Single(c => string.Equals(c.Code, code.Trim(), StringComparison.OrdinalIgnoreCase)));
    }

    public async Task<MajorCourseDto> AddExistingCourseToMajorAsync(long majorId, long courseId, int recommendedTerm, bool isRequired, CancellationToken cancellationToken = default)
    {
        var current = await curriculumService.GetAsync(majorId, cancellationToken);
        if (current.Courses.Any(c => c.CourseId == courseId)) { throw new BusinessException("این درس قبلاً به چارت رشته اضافه شده است."); }
        var courses = current.Courses.Select(c => new CurriculumCourseInput(c.CourseId, c.RecommendedTerm, c.IsRequired)).Append(new(courseId, recommendedTerm, isRequired)).ToList();
        var saved = await curriculumService.SaveAsync(majorId, courses, cancellationToken);
        return Map(saved.Courses.Single(c => c.CourseId == courseId));
    }

    public async Task RemoveCourseFromMajorAsync(long majorId, long courseId, CancellationToken cancellationToken = default)
    {
        var current = await curriculumService.GetAsync(majorId, cancellationToken);
        if (!current.Courses.Any(c => c.CourseId == courseId)) { throw new BusinessException("درس در چارت رشته وجود ندارد."); }
        var remaining = current.Courses.Where(c => c.CourseId != courseId).Select(c => new CurriculumCourseInput(c.CourseId, c.RecommendedTerm, c.IsRequired)).ToList();
        await curriculumService.SaveAsync(majorId, remaining, cancellationToken);
    }

    private static MajorCourseDto Map(MajorCurricula.MajorCourseDto c) => new(c.CourseId, c.Code, c.Title, c.Credits, c.RecommendedTerm, c.IsRequired);
}
