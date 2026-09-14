using MediatR;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.AdminPreRegistration.DTOs;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Features.AdminPreRegistration.Queries.GetCourseDemandSummary;

/// <summary>
/// Handles retrieving course demand summary for an academic term.
/// Aggregates submitted pre-registrations at the database level.
/// </summary>
public sealed class GetCourseDemandSummaryQueryHandler
    : IRequestHandler<GetCourseDemandSummaryQuery, IReadOnlyList<CourseDemandDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCourseDemandSummaryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<CourseDemandDto>> Handle(
        GetCourseDemandSummaryQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Verify academic term exists
        var termExists = await _context.AcademicTerms
            .AsNoTracking()
            .AnyAsync(t => t.Id == request.AcademicTermId, cancellationToken);

        if (!termExists)
        {
            throw new NotFoundException(nameof(AcademicTerm), request.AcademicTermId);
        }

        // 2. Aggregate demand at the database level for submitted requests only
        var rawDemand = await _context.StudentPreRegistrationItems
            .AsNoTracking()
            .Where(item => item.StudentPreRegistration.AcademicTermId == request.AcademicTermId
                        && item.StudentPreRegistration.Status == RequestStatus.Submitted)
            .GroupBy(item => new
            {
                item.CourseId,
                item.Course.Code,
                item.Course.Title,
                item.Course.Credits
            })
            .Select(g => new
            {
                g.Key.CourseId,
                CourseCode = g.Key.Code,
                CourseTitle = g.Key.Title,
                Credits = g.Key.Credits,
                StudentCount = g.Count(),
                AveragePriority = g.Average(x => (double)x.Priority)
            })
            .OrderByDescending(x => x.StudentCount)
            .ThenBy(x => x.CourseCode)
            .ToListAsync(cancellationToken);

        // 3. Map to DTOs
        var result = rawDemand
            .Select(d => new CourseDemandDto(
                d.CourseId,
                d.CourseCode,
                d.CourseTitle,
                d.StudentCount,
                Math.Round(d.AveragePriority, 2),
                d.StudentCount * d.Credits
            ))
            .ToList();

        return result;
    }
}
