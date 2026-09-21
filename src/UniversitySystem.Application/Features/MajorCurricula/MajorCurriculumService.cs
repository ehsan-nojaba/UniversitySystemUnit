using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Application.Common.Logic;

namespace UniversitySystem.Application.Features.MajorCurricula;

/// <summary>آموزش درس جدید یا موجود را به چارت رشته متصل می‌کند؛ حذف ارتباط نباید درخواست جاری دانشجو را نامعتبر کند.</summary>
public sealed class MajorCurriculumService(IMajorCurriculumRepository repository, IUnitOfWork unitOfWork)
{
    public Task<ICollection<MajorOptionDto>> GetMajorsAsync(CancellationToken ct)
    {
        return repository.GetMajorsAsync(ct);
    }

    public async Task<NextCourseCodeDto> GetNextCourseCodeAsync(long majorId, CancellationToken ct)
    {
        _ = await GetMajorAsync(majorId, false, ct);
        return new(await repository.GetNextCourseCodeAsync(ct));
    }

    private async Task<Major> GetMajorAsync(long majorId, bool editable, CancellationToken ct)
    {
        var major = await repository.GetMajorAsync(majorId, ct) ?? throw new NotFoundException("رشته انتخاب‌شده پیدا نشد.");
        if (editable && !major.IsActive) { throw new BusinessException("چارت رشته غیرفعال قابل ویرایش نیست."); }
        return major;
    }
    public async Task<MajorCurriculumDto> GetAsync(long majorId, CancellationToken ct)
    {
        var major = await GetMajorAsync(majorId, false, ct);
        return Map(major, await repository.GetCurrentAsync(majorId, ct));
    }
    private async Task<Curriculum> GetOrCreateAsync(Major major, CancellationToken ct)
    {
        var curriculum = await repository.GetCurrentAsync(major.Id, ct);
        if (curriculum is not null) { return curriculum; }
        var title = $"چارت {major.Title}";
        curriculum = CurriculumLogic.Create(major.Id,title[..Math.Min(200, title.Length)],"1");
        repository.Add(curriculum);
        await unitOfWork.SaveChangesAsync(ct);
        return curriculum;
    }
    public Task<MajorCurriculumDto> SaveAsync(long majorId, ICollection<CurriculumCourseInput> courses, CancellationToken ct)
    {
        return repository.ExecuteSerializableAsync(async () =>
        {
            var major = await GetMajorAsync(majorId, true, ct);
            if (courses is null || courses.Any(c => c.CourseId <= 0 || c.RecommendedTerm <= 0) || courses.Select(c => c.CourseId).Distinct().Count() != courses.Count) { throw new BusinessException("درس‌ها و ترم پیشنهادی باید معتبر و بدون تکرار باشند."); }
            var curriculum = await GetOrCreateAsync(major, ct);
            var ids = courses.Select(c => c.CourseId).ToList();
            var existingIds = curriculum.CurriculumCourses.Select(c => c.CourseId).ToList();
            var definitions = await repository.GetCoursesAsync(ids, ct);
            if (definitions.Count != ids.Count || definitions.Any(c => !c.IsActive && !existingIds.Contains(c.Id))) { throw new BusinessException("درس نامعتبر یا غیرفعال را نمی‌توان به چارت اضافه کرد."); }
            var removed = existingIds.Except(ids).ToList();
            if (removed.Count > 0 && await repository.HasCurrentStudentUseAsync(majorId, removed, ct)) { throw new BusinessException("درس دارای پیش‌انتخاب یا ثبت‌نام در ترم فعال را نمی‌توان از چارت این رشته حذف کرد."); }
            foreach (var id in removed) { CurriculumLogic.RemoveCourse(curriculum, id); }
            foreach (var item in courses)
            {
                if (existingIds.Contains(item.CourseId)) { CurriculumLogic.UpdateCourse(curriculum, item.CourseId, item.RecommendedTerm, item.IsRequired); }
                else { CurriculumLogic.AddCourse(curriculum, item.CourseId, item.RecommendedTerm, item.IsRequired); }
            }
            await unitOfWork.SaveChangesAsync(ct);
            return Map(major, curriculum, definitions);
        }, ct);
    }

    public Task<MajorCurriculumDto> CreateCourseAsync(long majorId, NewMajorCourseInput input, CancellationToken ct)
    {
        return repository.ExecuteSerializableAsync(async () =>
        {
            var major = await GetMajorAsync(majorId, true, ct);
            if (input is null || string.IsNullOrWhiteSpace(input.Title) || input.Title.Trim().Length > 200 || input.Credits <= 0 || input.Credits > 6 || input.RecommendedTerm <= 0) { throw new BusinessException("نام درس و ترم پیشنهادی باید معتبر و تعداد واحد بین ۱ تا ۶ باشد."); }
            var curriculum = await GetOrCreateAsync(major, ct);
            var code = await repository.GetNextCourseCodeAsync(ct);
            var course = CourseLogic.Create(code, input.Title.Trim(), input.Credits);
            repository.Add(course);
            await unitOfWork.SaveChangesAsync(ct);
            CurriculumLogic.AddCourse(curriculum, course.Id, input.RecommendedTerm, input.IsRequired);
            await unitOfWork.SaveChangesAsync(ct);
            var definitions = await repository.GetCoursesAsync(curriculum.CurriculumCourses.Select(c => c.CourseId).ToList(), ct);
            return Map(major, curriculum, definitions);
        }, ct);
    }

    private static MajorCurriculumDto Map(Major major, Curriculum? curriculum, ICollection<Course>? definitions = null)
    {
        return new MajorCurriculumDto(major.Id, major.Title, curriculum?.Id, curriculum?.Title, curriculum?.Version, curriculum?.CurriculumCourses.OrderBy(c => c.RecommendedTerm).ThenBy(c => c.CourseId).Select(c =>
        {
            var course = definitions?.Single(d => d.Id == c.CourseId) ?? c.Course;
            return new MajorCourseDto(course.Id, course.Code, course.Title, course.Credits, c.RecommendedTerm, c.IsRequired, course.IsActive);
        }).ToList() ?? []);
    }
}

