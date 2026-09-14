using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Repositories;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Features.CourseOfferings.Services;

public sealed class CourseOfferingService(ICourseOfferingRepository repository, IUnitOfWork unitOfWork) : ICourseOfferingService
{
    public async Task<ICollection<CourseOfferingDto>> GetOfferingsByTermAsync(long academicTermId, CancellationToken cancellationToken = default)
    {
        var term = await repository.GetAcademicTermAsync(academicTermId, cancellationToken);
        if (term is null) throw new NotFoundException(nameof(AcademicTerm), academicTermId);

        return await repository.GetOfferingsByTermAsync(academicTermId, cancellationToken);
    }

    public async Task<CourseOfferingDto> CreateOfferingAsync(long academicTermId, long courseId, int capacity, CancellationToken cancellationToken = default)
    {
        var term = await repository.GetAcademicTermAsync(academicTermId, cancellationToken);
        if (term is null) throw new NotFoundException(nameof(AcademicTerm), academicTermId);
        if (!term.IsActive) throw new BusinessException("ترم تحصیلی انتخابی فعال نیست.");

        var course = await repository.GetCourseAsync(courseId, cancellationToken);
        if (course is null) throw new NotFoundException(nameof(Course), courseId);
        if (!course.IsActive) throw new BusinessException("درس انتخابی فعال نیست.");

        var exists = await repository.OfferingExistsAsync(academicTermId, courseId, cancellationToken);
        if (exists) throw new BusinessException("برای این درس در این ترم تحصیلی قبلاً ارائه ثبت شده است.");

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
            IsActive = offering.IsActive
        };
    }

    public async Task<CourseOfferingDto> UpdateOfferingAsync(long id, int capacity, bool? isActive, CancellationToken cancellationToken = default)
    {
        var offering = await repository.GetByIdWithCourseAsync(id, cancellationToken);
        if (offering is null) throw new NotFoundException(nameof(CourseOffering), id);

        offering.UpdateCapacity(capacity);
        if (isActive.HasValue)
        {
            if (isActive.Value) offering.Activate();
            else offering.Deactivate();
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
            IsActive = offering.IsActive
        };
    }
}
