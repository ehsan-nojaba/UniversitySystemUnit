using StudentPreRegistrationEntity = UniversitySystem.Domain.Entities.StudentPreRegistration;
using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Application.Features.StudentPreRegistration.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;
using UniversitySystem.Application.Common.Logic;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Services;
/// <summary>
/// اجرای قواعد و هماهنگی عملیات بخش «پیش‌انتخاب واحد دانشجو»؛ داده را از ریپازیتوری می‌گیرد و تغییرات را از طریق مدل‌های دامنه انجام می‌دهد.
/// </summary>
public sealed class StudentPreRegistrationService(IStudentPreRegistrationRepository repository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IStudentCourseEligibilityService eligibilityService, IDateTimeProvider dateTimeProvider) : IStudentPreRegistrationService
{
    private async Task<Student> GetCurrentStudentAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.UserId) || !long.TryParse(currentUserService.UserId, out var userId))
        {
            throw new UnauthorizedAccessException("کاربر جاری احراز هویت نشده است.");
        }

        var student = await repository.GetStudentByUserIdAsync(userId, cancellationToken);
        if (student is null)
        {
            throw new NotFoundException("پروفایل دانشجویی برای کاربر جاری یافت نشد.");
        }

        return student;
    }

    private async Task<AcademicTerm> GetActiveTermAsync(long termId, CancellationToken cancellationToken)
    {
        var term = await repository.GetAcademicTermAsync(termId, cancellationToken);
        if (term is null)
        {
            throw new NotFoundException(nameof(AcademicTerm), termId);
        }

        if (!term.IsActive)
        {
            throw new BusinessException("ترم تحصیلی انتخابی فعال نیست.");
        }

        return term;
    }

    public async Task<StudentPreRegistrationDto> SaveDraftAsync(long academicTermId, ICollection<SelectedCourseItemDto> courses, CancellationToken cancellationToken = default)
    {
        if (academicTermId <= 0)
        {
            throw new BusinessException("شناسه ترم تحصیلی نامعتبر است.");
        }

        if (courses is null || courses.Count == 0)
        {
            throw new BusinessException("حداقل یک درس باید برای پیش‌ثبت‌نام انتخاب شود.");
        }

        if (courses.Any(course => course.CourseId <= 0))
        {
            throw new BusinessException("شناسه درس نامعتبر است.");
        }

        if (courses.Any(course => course.Priority <= 0))
        {
            throw new BusinessException("اولویت درس باید بزرگتر از صفر باشد.");
        }

        if (courses.Select(course => course.CourseId).Distinct().Count() != courses.Count)
        {
            throw new BusinessException("انتخاب درس‌های تکراری مجاز نیست.");
        }

        var student = await GetCurrentStudentAsync(cancellationToken);
        await GetActiveTermAsync(academicTermId, cancellationToken);
        var eligibleCourses = await eligibilityService.GetEligibleCoursesAsync(student.Id, academicTermId, cancellationToken);
        var eligibleMap = eligibleCourses.ToDictionary(c => c.CourseId);
        foreach (var course in courses)
            if (!eligibleMap.ContainsKey(course.CourseId))
            {
                throw new BusinessException($"درس با شناسه {course.CourseId} جزو درس‌های قابل انتخاب برای این ترم نیست.");
            }

        var preRegistration = await repository.GetPreRegistrationWithItemsAsync(student.Id, academicTermId, cancellationToken);
        if (preRegistration is null)
        {
            preRegistration = StudentPreRegistrationLogic.Create(student.Id,academicTermId);
            repository.AddPreRegistration(preRegistration);
            foreach (var item in courses)
                StudentPreRegistrationLogic.AddCourse(                preRegistration,item.CourseId,item.Priority);
        }
        else
        {
            if (preRegistration.Status != RequestStatus.Draft)
            {
                throw new BusinessException("امکان ویرایش پیش‌ثبت‌نامی که در وضعیت پیش‌نویس (Draft) نیست وجود ندارد.");
            }

            var requestedCourseIds = courses.Select(c => c.CourseId).ToHashSet();
            var itemsToRemove = ( from i in preRegistration.Items where !requestedCourseIds.Contains(i.CourseId)select i).ToList();
            foreach (var toRemove in itemsToRemove)
                StudentPreRegistrationLogic.RemoveCourse(                preRegistration,toRemove.CourseId);
            var existingItems = preRegistration.Items.ToDictionary(i => i.CourseId);
            foreach (var item in courses)
            {
                if (existingItems.TryGetValue(item.CourseId, out var existingItem))
                {
                    if (existingItem.Priority != item.Priority)
                    {
                        StudentPreRegistrationLogic.UpdateCoursePriority(                        preRegistration,item.CourseId,item.Priority);
                    }
                }
                else
                {
                    StudentPreRegistrationLogic.AddCourse(                    preRegistration,item.CourseId,item.Priority);
                }
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        var responseCourses = ( from c in courses orderby c.Priority let info = eligibleMap[c.CourseId] select new PreRegistrationCourseItemDto { CourseId = c.CourseId, Code = info.Code, Title = info.Title, Credits = info.Credits, Priority = c.Priority }  ).ToList();
        return new StudentPreRegistrationDto
        {
            PreRegistrationId = preRegistration.Id,
            AcademicTermId = preRegistration.AcademicTermId,
            Status = preRegistration.Status.ToString(),
            Courses = responseCourses,
            TotalCredits = responseCourses.Sum(c => c.Credits)
        };
    }

    public async Task<StudentPreRegistrationDto> SubmitAsync(long academicTermId, CancellationToken cancellationToken = default)
    {
        if (academicTermId <= 0)
        {
            throw new BusinessException("شناسه ترم تحصیلی نامعتبر است.");
        }

        var student = await GetCurrentStudentAsync(cancellationToken);
        await GetActiveTermAsync(academicTermId, cancellationToken);
        var preRegistration = await repository.GetPreRegistrationWithItemsAndCoursesAsync(student.Id, academicTermId, cancellationToken);
        if (preRegistration is null)
        {
            throw new NotFoundException("پیش‌ثبت‌نامی برای این ترم تحصیلی یافت نشد.");
        }

        if (preRegistration.Status != RequestStatus.Draft)
        {
            throw new BusinessException("فقط پیش‌ثبت‌نام در وضعیت پیش‌نویس (Draft) قابل ثبت نهایی است.");
        }

        if (preRegistration.Items.Count == 0)
        {
            throw new BusinessException("برای ثبت نهایی، باید حداقل یک درس انتخاب شده باشد.");
        }

        var eligibleCourses = await eligibilityService.GetEligibleCoursesAsync(student.Id, academicTermId, cancellationToken);
        var eligibleMap = eligibleCourses.ToDictionary(c => c.CourseId);
        foreach (var item in preRegistration.Items)
            if (!eligibleMap.ContainsKey(item.CourseId))
            {
                throw new BusinessException($"درس '{item.Course.Title}' دیگر برای این ترم قابل انتخاب نیست.");
            }

        StudentPreRegistrationLogic.Submit(
        preRegistration,dateTimeProvider.UtcNow);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var courseItems = ( from i in preRegistration.Items orderby i.Priority select new PreRegistrationCourseItemDto { CourseId = i.CourseId, Code = i.Course.Code, Title = i.Course.Title, Credits = i.Course.Credits, Priority = i.Priority }  ).ToList();
        return new StudentPreRegistrationDto
        {
            PreRegistrationId = preRegistration.Id,
            AcademicTermId = preRegistration.AcademicTermId,
            Status = preRegistration.Status.ToString(),
            Courses = courseItems,
            TotalCredits = courseItems.Sum(c => c.Credits),
            SubmittedAt = preRegistration.SubmittedAt
        };
    }

    public async Task<StudentPreRegistrationDto?> GetByTermAsync(long academicTermId, CancellationToken cancellationToken = default)
    {
        if (academicTermId <= 0)
        {
            throw new BusinessException("شناسه ترم تحصیلی نامعتبر است.");
        }

        var student = await GetCurrentStudentAsync(cancellationToken);
        var preRegistration = await repository.GetPreRegistrationWithItemsAndCoursesAsync(student.Id, academicTermId, cancellationToken);
        if (preRegistration is null)
        {
            return null;
        }

        var courseItems = ( from i in preRegistration.Items orderby i.Priority select new PreRegistrationCourseItemDto { CourseId = i.CourseId, Code = i.Course.Code, Title = i.Course.Title, Credits = i.Course.Credits, Priority = i.Priority }  ).ToList();
        return new StudentPreRegistrationDto
        {
            PreRegistrationId = preRegistration.Id,
            AcademicTermId = preRegistration.AcademicTermId,
            Status = preRegistration.Status.ToString(),
            Courses = courseItems,
            TotalCredits = courseItems.Sum(c => c.Credits),
            SubmittedAt = preRegistration.SubmittedAt
        };
    }

    public async Task<ICollection<EligibleCourseDto>> GetEligibleCoursesAsync(long academicTermId, CancellationToken cancellationToken = default)
    {
        if (academicTermId <= 0)
        {
            throw new BusinessException("شناسه ترم تحصیلی معتبر نیست.");
        }

        var student = await GetCurrentStudentAsync(cancellationToken);
        var term = await repository.GetAcademicTermAsync(academicTermId, cancellationToken);
        if (term is null)
        {
            throw new NotFoundException(nameof(AcademicTerm), academicTermId);
        }

        return await eligibilityService.GetEligibleCoursesAsync(student.Id, academicTermId, cancellationToken);
    }
}

