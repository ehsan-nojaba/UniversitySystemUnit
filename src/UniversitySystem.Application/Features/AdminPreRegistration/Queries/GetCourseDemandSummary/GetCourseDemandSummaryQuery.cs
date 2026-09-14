using MediatR;
using UniversitySystem.Application.Features.AdminPreRegistration.DTOs;

namespace UniversitySystem.Application.Features.AdminPreRegistration.Queries.GetCourseDemandSummary;

/// <summary>
/// Query to retrieve aggregated student demand per course for a given academic term.
/// Only submitted pre-registrations are considered.
/// </summary>
public sealed record GetCourseDemandSummaryQuery(long AcademicTermId)
    : IRequest<IReadOnlyList<CourseDemandDto>>;
