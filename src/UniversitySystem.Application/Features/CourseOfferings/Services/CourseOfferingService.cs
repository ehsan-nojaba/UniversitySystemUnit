using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Repositories;
using UniversitySystem.Application.Features.Enrollments.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Application.Common.Logic;

namespace UniversitySystem.Application.Features.CourseOfferings.Services;
/// <summary>
/// اجرای قواعد و هماهنگی عملیات بخش «ارائه درس و برنامه زمانی کلاس»؛ داده را از ریپازیتوری می‌گیرد و تغییرات را از طریق مدل‌های دامنه انجام می‌دهد.
/// </summary>
public sealed class CourseOfferingService(ICourseOfferingRepository repository, IUnitOfWork unitOfWork, IProfessorScheduleConflictChecker conflictChecker, IEnrollmentRepository enrollmentRepository) : ICourseOfferingService
{
    public async Task<ICollection<CourseOfferingDto>> GetOfferingsByTermAsync(long academicTermId, CancellationToken cancellationToken = default)
    {
        if (academicTermId <= 0)
        {
            throw new BusinessException("شناسه ترم تحصیلی نامعتبر است.");
        }

        var term = await repository.GetAcademicTermAsync(academicTermId, cancellationToken);
        if (term is null)
        {
            throw new NotFoundException(nameof(AcademicTerm), academicTermId);
        }

        return await repository.GetOfferingsByTermAsync(academicTermId, cancellationToken);
    }

    public async Task<CourseOfferingDto> CreateOfferingAsync(long academicTermId, long courseId, int capacity, CancellationToken cancellationToken = default)
    {
        if (academicTermId <= 0)
        {
            throw new BusinessException("شناسه ترم تحصیلی نامعتبر است.");
        }

        if (courseId <= 0)
        {
            throw new BusinessException("شناسه درس نامعتبر است.");
        }

        if (capacity <= 0)
        {
            throw new BusinessException("ظرفیت کلاس باید یک عدد مثبت باشد.");
        }

        var term = await repository.GetAcademicTermAsync(academicTermId, cancellationToken);
        if (term is null)
        {
            throw new NotFoundException(nameof(AcademicTerm), academicTermId);
        }

        if (!term.IsActive)
        {
            throw new BusinessException("ترم تحصیلی انتخابی فعال نیست.");
        }

        var course = await repository.GetCourseAsync(courseId, cancellationToken);
        if (course is null)
        {
            throw new NotFoundException(nameof(Course), courseId);
        }

        if (!course.IsActive)
        {
            throw new BusinessException("درس انتخابی فعال نیست.");
        }

        var exists = await repository.OfferingExistsAsync(academicTermId, courseId, cancellationToken);
        if (exists)
        {
            throw new BusinessException("برای این درس در این ترم تحصیلی قبلاً ارائه ثبت شده است.");
        }

        var offering = CourseOfferingLogic.Create(courseId,academicTermId,capacity);
        repository.Add(offering);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new CourseOfferingDto
        {
            CourseOfferingId = offering.Id,
            AcademicTermId = offering.AcademicTermId,
            CourseId = offering.CourseId,
            CourseCode = course.Code,
            CourseTitle = course.Title,
            Capacity = offering.Capacity,
            IsActive = offering.IsActive,
            IsFinalized = offering.IsFinalized
        };
    }

    public async Task<CourseOfferingDto> UpdateOfferingAsync(long id, int capacity, bool? isActive, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            throw new BusinessException("شناسه ارائه درس نامعتبر است.");
        }

        if (capacity <= 0)
        {
            throw new BusinessException("ظرفیت کلاس باید یک عدد مثبت باشد.");
        }

        var offering = await repository.GetByIdWithCourseAsync(id, cancellationToken);
        if (offering is null)
        {
            throw new NotFoundException(nameof(CourseOffering), id);
        }

        if (offering.IsFinalized) { throw new BusinessException("ابتدا ارائه را به برنامه‌ریزی برگردانید."); }
        if (await enrollmentRepository.GetEnrollmentCountAsync(id, cancellationToken) > capacity)
        {
            throw new BusinessException("Capacity cannot be less than the number of enrolled students.");
        }

        CourseOfferingLogic.UpdateCapacity(
        offering,capacity);
        if (isActive.HasValue)
        {
            if (isActive.Value)
            {
                CourseOfferingLogic.Activate(                offering);
            }
            else
            {
                CourseOfferingLogic.Deactivate(                offering);
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new CourseOfferingDto
        {
            CourseOfferingId = offering.Id,
            AcademicTermId = offering.AcademicTermId,
            CourseId = offering.CourseId,
            CourseCode = offering.Course.Code,
            CourseTitle = offering.Course.Title,
            Capacity = offering.Capacity,
            IsActive = offering.IsActive,
            IsFinalized = offering.IsFinalized
        };
    }

    public async Task<ICollection<CourseOfferingScheduleDto>> GetScheduleAsync(long courseOfferingId, CancellationToken cancellationToken = default)
    {
        if (courseOfferingId <= 0)
        {
            throw new BusinessException("شناسه ارائه درس نامعتبر است.");
        }

        var offering = await repository.GetByIdAsync(courseOfferingId, cancellationToken);
        if (offering is null)
        {
            throw new NotFoundException(nameof(CourseOffering), courseOfferingId);
        }

        return await repository.GetSchedulesByOfferingIdAsync(courseOfferingId, cancellationToken);
    }

    public async Task<ICollection<CourseOfferingScheduleDto>> SaveScheduleAsync(long courseOfferingId, ICollection<CourseOfferingScheduleSlotDto> slots, CancellationToken cancellationToken = default)
    {
        if (courseOfferingId <= 0)
        {
            throw new BusinessException("شناسه ارائه درس نامعتبر است.");
        }

        ValidateScheduleSlots(slots);

        var offering = await repository.GetByIdAsync(courseOfferingId, cancellationToken);
        if (offering is null)
        {
            throw new NotFoundException(nameof(CourseOffering), courseOfferingId);
        }

        var term = await repository.GetAcademicTermAsync(offering.AcademicTermId, cancellationToken);
        if (!offering.IsActive || term is null || !term.IsActive)
        {
            throw new BusinessException("Offering and academic term must be active.");
        }

        if (offering.IsFinalized) { throw new BusinessException("ابتدا ارائه را به برنامه‌ریزی برگردانید."); }
        await conflictChecker.CheckConflictsAsync(courseOfferingId, slots, cancellationToken);
        await repository.SaveSchedulesAsync(courseOfferingId, slots, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return await repository.GetSchedulesByOfferingIdAsync(courseOfferingId, cancellationToken);
    }

    private static void ValidateScheduleSlots(ICollection<CourseOfferingScheduleSlotDto>? slots)
    {
        if (slots is null)
        {
            throw new BusinessException("برنامه زمانی نمی‌تواند خالی باشد.");
        }

        if (slots.Any(slot => !Enum.IsDefined(slot.DayOfWeek) || slot.StartTime >= slot.EndTime))
        {
            throw new BusinessException("روز هفته و بازه زمانی کلاس معتبر نیست.");
        }

        var values = slots.ToList();

        if (values.GroupBy(slot => new { slot.DayOfWeek, slot.StartTime, slot.EndTime }).Any(group => group.Count() > 1))
        {
            throw new BusinessException("اسلات تکراری مجاز نیست.");
        }

        for (var i = 0; i < values.Count; i++)
        {
            for (var j = i + 1; j < values.Count; j++)
            {
                if (values[i].DayOfWeek == values[j].DayOfWeek && values[i].StartTime < values[j].EndTime && values[j].StartTime < values[i].EndTime)
                {
                    throw new BusinessException("در یک ارائه دو اسلات متداخل در یک روز مجاز نیست.");
                }
            }
        }
    }
}

