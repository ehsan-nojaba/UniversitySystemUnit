using StudentPreRegistrationEntity = UniversitySystem.Domain.Entities.StudentPreRegistration;
using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Common.Logic;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Application.Features.StudentPreRegistration.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Services;

/// <summary>
/// قواعد پیش‌انتخاب دانشجو را اجرا می‌کند و ذخیره‌سازی را به ریپازیتوری و واحدکار می‌سپارد.
/// </summary>
public sealed class StudentPreRegistrationService(
    IStudentPreRegistrationRepository repository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IStudentCourseEligibilityService eligibilityService,
    IDateTimeProvider dateTimeProvider) : IStudentPreRegistrationService
{
    private const int MaxAttempts = 2;

    public async Task<StudentPreRegistrationDto> SaveDraftAsync(long academicTermId, ICollection<SelectedCourseItemDto> courses, CancellationToken cancellationToken = default)
    {
        ValidateDraft(academicTermId, courses);
        var student = await GetCurrentStudentAsync(cancellationToken);
        await GetActiveTermAsync(academicTermId, cancellationToken);
        var eligibleMap = await GetEligibleMapAsync(student.Id, academicTermId, cancellationToken);
        EnsureCoursesAreEligible(courses, eligibleMap);

        var preRegistration = await repository.GetPreRegistrationWithItemsAsync(student.Id, academicTermId, cancellationToken);
        if (preRegistration is null)
        {
            preRegistration = StudentPreRegistrationLogic.Create(student.Id, academicTermId);
            preRegistration.AttemptCount = 1;
            repository.AddPreRegistration(preRegistration);
            foreach (var course in courses)
            {
                StudentPreRegistrationLogic.AddCourse(preRegistration, course.CourseId, course.Priority);
            }
        }
        else
        {
            EnsureDraft(preRegistration);
            preRegistration.AttemptCount = Math.Max(1, preRegistration.AttemptCount);
            var requestedCourseIds = courses.Select(course => course.CourseId).ToHashSet();
            var removedItems = preRegistration.Items.Where(item => !requestedCourseIds.Contains(item.CourseId)).ToList();
            foreach (var item in removedItems)
            {
                StudentPreRegistrationLogic.RemoveCourse(preRegistration, item.CourseId);
            }

            var existingItems = preRegistration.Items.ToDictionary(item => item.CourseId);
            foreach (var course in courses)
            {
                if (existingItems.TryGetValue(course.CourseId, out var existingItem))
                {
                    if (existingItem.Priority != course.Priority)
                    {
                        StudentPreRegistrationLogic.UpdateCoursePriority(preRegistration, course.CourseId, course.Priority);
                    }
                }
                else
                {
                    StudentPreRegistrationLogic.AddCourse(preRegistration, course.CourseId, course.Priority);
                }
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        var responseCourses = courses.OrderBy(course => course.Priority).Select(course => ToCourseItem(course, eligibleMap[course.CourseId])).ToList();
        return ToDto(preRegistration, responseCourses);
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

        EnsureDraft(preRegistration);
        if (preRegistration.Items.Count == 0)
        {
            throw new BusinessException("برای ثبت نهایی، باید حداقل یک درس انتخاب شده باشد.");
        }

        var eligibleMap = await GetEligibleMapAsync(student.Id, academicTermId, cancellationToken);
        foreach (var item in preRegistration.Items)
        {
            if (!eligibleMap.ContainsKey(item.CourseId))
            {
                throw new BusinessException($"درس '{item.Course.Title}' دیگر برای این ترم قابل انتخاب نیست.");
            }
        }

        preRegistration.AttemptCount = Math.Clamp(Math.Max(1, preRegistration.AttemptCount), 1, MaxAttempts);
        StudentPreRegistrationLogic.Submit(preRegistration, dateTimeProvider.UtcNow);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var courseItems = preRegistration.Items.OrderBy(item => item.Priority).Select(ToCourseItem).ToList();
        return ToDto(preRegistration, courseItems);
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

        var courseItems = preRegistration.Items.OrderBy(item => item.Priority).Select(ToCourseItem).ToList();
        return ToDto(preRegistration, courseItems);
    }

    public async Task<ICollection<EligibleCourseDto>> GetEligibleCoursesAsync(long academicTermId, CancellationToken cancellationToken = default)
    {
        if (academicTermId <= 0)
        {
            throw new BusinessException("شناسه ترم تحصیلی معتبر نیست.");
        }

        var student = await GetCurrentStudentAsync(cancellationToken);
        await GetAcademicTermAsync(academicTermId, cancellationToken);
        var preRegistration = await repository.GetPreRegistrationWithItemsAsync(student.Id, academicTermId, cancellationToken);
        if (preRegistration is not null && preRegistration.Status == RequestStatus.Submitted && AttemptCountOf(preRegistration) >= MaxAttempts)
        {
            throw new PreRegistrationLimitException("سهمیه دو نوبت پیش‌انتخاب این ترم برای شما تکمیل شده است و امکان ورود دوباره وجود ندارد.");
        }

        return await eligibilityService.GetEligibleCoursesAsync(student.Id, academicTermId, cancellationToken);
    }

    public async Task<StudentPreRegistrationDto> StartNewAttemptAsync(long academicTermId, CancellationToken cancellationToken = default)
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

        var attemptCount = AttemptCountOf(preRegistration);
        if (preRegistration.Status != RequestStatus.Submitted)
        {
            throw new BusinessException("نوبت جدید فقط بعد از ارسال نهایی نوبت قبلی قابل شروع است.");
        }

        if (attemptCount >= MaxAttempts)
        {
            throw new PreRegistrationLimitException("سهمیه دو نوبت پیش‌انتخاب این ترم برای شما تکمیل شده است.");
        }

        preRegistration.AttemptCount = attemptCount + 1;
        preRegistration.Status = RequestStatus.Draft;
        preRegistration.SubmittedAt = null;
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ToDto(preRegistration, preRegistration.Items.OrderBy(item => item.Priority).Select(ToCourseItem).ToList());
    }

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
        var term = await GetAcademicTermAsync(termId, cancellationToken);
        if (!term.IsActive)
        {
            throw new BusinessException("ترم تحصیلی انتخابی فعال نیست.");
        }

        return term;
    }

    private async Task<AcademicTerm> GetAcademicTermAsync(long termId, CancellationToken cancellationToken)
    {
        var term = await repository.GetAcademicTermAsync(termId, cancellationToken);
        if (term is null)
        {
            throw new NotFoundException(nameof(AcademicTerm), termId);
        }

        return term;
    }

    private async Task<Dictionary<long, EligibleCourseDto>> GetEligibleMapAsync(long studentId, long academicTermId, CancellationToken cancellationToken)
    {
        return (await eligibilityService.GetEligibleCoursesAsync(studentId, academicTermId, cancellationToken)).ToDictionary(course => course.CourseId);
    }

    private static void ValidateDraft(long academicTermId, ICollection<SelectedCourseItemDto> courses)
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
    }

    private static void EnsureCoursesAreEligible(ICollection<SelectedCourseItemDto> courses, IReadOnlyDictionary<long, EligibleCourseDto> eligibleMap)
    {
        foreach (var course in courses)
        {
            if (!eligibleMap.ContainsKey(course.CourseId))
            {
                throw new BusinessException($"درس با شناسه {course.CourseId} جزو درس‌های قابل انتخاب برای این ترم نیست.");
            }
        }
    }

    private static void EnsureDraft(StudentPreRegistrationEntity preRegistration)
    {
        if (preRegistration.Status != RequestStatus.Draft)
        {
            throw new BusinessException("فقط پیش‌انتخاب در وضعیت پیش‌نویس قابل ویرایش یا ارسال است.");
        }
    }

    private static int AttemptCountOf(StudentPreRegistrationEntity preRegistration)
    {
        return preRegistration.AttemptCount > 0 ? preRegistration.AttemptCount : preRegistration.Status == RequestStatus.Submitted ? 1 : 0;
    }

    private static int RemainingAttemptsOf(StudentPreRegistrationEntity preRegistration)
    {
        return Math.Max(0, MaxAttempts - AttemptCountOf(preRegistration));
    }

    private static StudentPreRegistrationDto ToDto(StudentPreRegistrationEntity preRegistration, ICollection<PreRegistrationCourseItemDto> courses)
    {
        return new StudentPreRegistrationDto
        {
            PreRegistrationId = preRegistration.Id,
            AcademicTermId = preRegistration.AcademicTermId,
            Status = preRegistration.Status.ToString(),
            AttemptCount = AttemptCountOf(preRegistration),
            RemainingAttempts = RemainingAttemptsOf(preRegistration),
            Courses = courses,
            TotalCredits = courses.Sum(course => course.Credits),
            SubmittedAt = preRegistration.SubmittedAt
        };
    }

    private static PreRegistrationCourseItemDto ToCourseItem(SelectedCourseItemDto selectedCourse, EligibleCourseDto course)
    {
        return new PreRegistrationCourseItemDto
        {
            CourseId = course.CourseId,
            Code = course.Code,
            Title = course.Title,
            Credits = course.Credits,
            Priority = selectedCourse.Priority
        };
    }

    private static PreRegistrationCourseItemDto ToCourseItem(StudentPreRegistrationItem item)
    {
        return new PreRegistrationCourseItemDto
        {
            CourseId = item.CourseId,
            Code = item.Course.Code,
            Title = item.Course.Title,
            Credits = item.Course.Credits,
            Priority = item.Priority
        };
    }
}
