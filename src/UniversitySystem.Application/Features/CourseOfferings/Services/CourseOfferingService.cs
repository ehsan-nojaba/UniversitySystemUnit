using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Repositories;
using UniversitySystem.Application.Features.Enrollments.Repositories;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Features.CourseOfferings.Services;
/// <summary>
/// اجرای قواعد و هماهنگی عملیات بخش «ارائه درس و برنامه زمانی کلاس»؛ داده را از ریپازیتوری می‌گیرد و تغییرات را از طریق مدل‌های دامنه انجام می‌دهد.
/// </summary>
public sealed class CourseOfferingService(ICourseOfferingRepository repository, IUnitOfWork unitOfWork, IProfessorScheduleConflictChecker conflictChecker, IEnrollmentRepository enrollmentRepository) : ICourseOfferingService
{
    public async Task<ICollection<CourseOfferingDto>> GetOfferingsByTermAsync(long academicTermId, CancellationToken cancellationToken = default)
    {
        var term = await repository.GetAcademicTermAsync(academicTermId, cancellationToken);
        if (term is null)
        {
            throw new NotFoundException(nameof(AcademicTerm), academicTermId);
        }

        return await repository.GetOfferingsByTermAsync(academicTermId, cancellationToken);
    }

    public async Task<CourseOfferingDto> CreateOfferingAsync(long academicTermId, long courseId, int capacity, CancellationToken cancellationToken = default)
    {
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

        var offering = new CourseOffering(courseId, academicTermId, capacity);
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

        offering.UpdateCapacity(capacity);
        if (isActive.HasValue)
        {
            if (isActive.Value)
            {
                offering.Activate();
            }
            else
            {
                offering.Deactivate();
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
        var offering = await repository.GetByIdAsync(courseOfferingId, cancellationToken);
        if (offering is null)
        {
            throw new NotFoundException(nameof(CourseOffering), courseOfferingId);
        }

        return await repository.GetSchedulesByOfferingIdAsync(courseOfferingId, cancellationToken);
    }

    public async Task<ICollection<CourseOfferingScheduleDto>> SaveScheduleAsync(long courseOfferingId, IReadOnlyCollection<CourseOfferingScheduleSlotDto> slots, CancellationToken cancellationToken = default)
    {
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
}
