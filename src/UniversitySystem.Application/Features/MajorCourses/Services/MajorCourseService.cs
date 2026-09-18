using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.MajorCourses.DTOs;
using UniversitySystem.Application.Features.MajorCourses.Repositories;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Features.MajorCourses.Services;

/// <summary>
/// اجرای قواعد و هماهنگی عملیات بخش «مدیریت دروس رشته»؛ داده را از ریپازیتوری می‌گیرد و تغییرات را از طریق مدل‌های دامنه انجام می‌دهد.
/// </summary>
public sealed class MajorCourseService(IMajorCourseRepository repository, IUnitOfWork unitOfWork) : IMajorCourseService
{
    public Task<IReadOnlyCollection<MajorOptionDto>> GetMajorsAsync(CancellationToken cancellationToken = default)
        => repository.GetActiveMajorsAsync(cancellationToken);

    public async Task<IReadOnlyCollection<MajorCourseDto>> GetCoursesForMajorAsync(long majorId, CancellationToken cancellationToken = default)
    {
        _ = await repository.GetMajorByIdAsync(majorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Major), majorId);

        var curriculum = await repository.GetActiveCurriculumForMajorAsync(majorId, cancellationToken)
            ?? throw new BusinessException("رشته انتخابی چارت درسی فعالی ندارد.");

        return await repository.GetCurriculumCoursesAsync(curriculum.Id, cancellationToken);
    }

    public async Task<MajorCourseDto> CreateCourseForMajorAsync(long majorId, string code, string title, int credits, int recommendedTerm, bool isRequired, CancellationToken cancellationToken = default)
    {
        var major = await repository.GetMajorByIdAsync(majorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Major), majorId);

        if (!major.IsActive)
        {
            throw new BusinessException("رشته انتخابی فعال نیست.");
        }

        var curriculum = await repository.GetActiveCurriculumForMajorAsync(majorId, cancellationToken)
            ?? throw new BusinessException("رشته انتخابی چارت درسی فعالی ندارد.");

        if (await repository.CourseExistsByCodeAsync(code, cancellationToken))
        {
            throw new BusinessException("درسی با این کد قبلاً ثبت شده است.");
        }

        var course = new Course(code, title, credits);
        repository.AddCourse(course);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        curriculum.AddCourse(course.Id, recommendedTerm, isRequired);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new MajorCourseDto(course.Id, course.Code, course.Title, course.Credits, recommendedTerm, isRequired);
    }

    public async Task<MajorCourseDto> AddExistingCourseToMajorAsync(long majorId, long courseId, int recommendedTerm, bool isRequired, CancellationToken cancellationToken = default)
    {
        var major = await repository.GetMajorByIdAsync(majorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Major), majorId);

        if (!major.IsActive)
        {
            throw new BusinessException("رشته انتخابی فعال نیست.");
        }

        var curriculum = await repository.GetActiveCurriculumForMajorAsync(majorId, cancellationToken)
            ?? throw new BusinessException("رشته انتخابی چارت درسی فعالی ندارد.");

        var course = await repository.GetCourseByIdAsync(courseId, cancellationToken)
            ?? throw new NotFoundException(nameof(Course), courseId);

        if (!course.IsActive)
        {
            throw new BusinessException("درس انتخابی فعال نیست.");
        }

        if (await repository.CurriculumCourseExistsAsync(curriculum.Id, courseId, cancellationToken))
        {
            throw new BusinessException("این درس قبلاً به چارت این رشته اضافه شده است.");
        }

        curriculum.AddCourse(courseId, recommendedTerm, isRequired);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new MajorCourseDto(course.Id, course.Code, course.Title, course.Credits, recommendedTerm, isRequired);
    }

    public async Task RemoveCourseFromMajorAsync(long majorId, long courseId, CancellationToken cancellationToken = default)
    {
        _ = await repository.GetMajorByIdAsync(majorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Major), majorId);

        var curriculum = await repository.GetActiveCurriculumForMajorAsync(majorId, cancellationToken)
            ?? throw new BusinessException("رشته انتخابی چارت درسی فعالی ندارد.");

        if (!await repository.CurriculumCourseExistsAsync(curriculum.Id, courseId, cancellationToken))
        {
            throw new BusinessException("این درس در چارت این رشته وجود ندارد.");
        }

        await repository.RemoveCurriculumCourseAsync(curriculum.Id, courseId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
