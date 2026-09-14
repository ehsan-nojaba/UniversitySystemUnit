using MediatR;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.AdminPlanning.DTOs;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Features.AdminPlanning.Queries.GetAcademicPlanningOverview;

public sealed class GetAcademicPlanningOverviewQueryHandler
    : IRequestHandler<GetAcademicPlanningOverviewQuery, AcademicPlanningOverviewDto>
{
    private readonly IApplicationDbContext _context;

    public GetAcademicPlanningOverviewQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<AcademicPlanningOverviewDto> Handle(GetAcademicPlanningOverviewQuery request, CancellationToken cancellationToken)
    {
        var term = await (
            from t in _context.AcademicTerms.AsNoTracking()
            where t.Id == request.AcademicTermId
            select new { t.Id, t.Code, t.Title }
        ).FirstOrDefaultAsync(cancellationToken);
        if (term is null) throw new NotFoundException(nameof(AcademicTerm), request.AcademicTermId);

        var studentDemand = await (
            from item in _context.StudentPreRegistrationItems.AsNoTracking()
            join preReg in _context.StudentPreRegistrations.AsNoTracking() on item.StudentPreRegistrationId equals preReg.Id
            where preReg.AcademicTermId == request.AcademicTermId && preReg.Status == RequestStatus.Submitted
            group item by item.CourseId into g
            select new { CourseId = g.Key, DemandCount = g.Count() }
        ).ToListAsync(cancellationToken);

        var professorInterests = await (
            from reqCourse in _context.ProfessorTeachingRequestCourses.AsNoTracking()
            join req in _context.ProfessorTeachingRequests.AsNoTracking() on reqCourse.ProfessorTeachingRequestId equals req.Id
            join prof in _context.Professors.AsNoTracking() on req.ProfessorId equals prof.Id
            join user in _context.Users.AsNoTracking() on prof.UserId equals user.Id
            where req.AcademicTermId == request.AcademicTermId && req.Status == RequestStatus.Submitted
            select new
            {
                reqCourse.CourseId,
                req.ProfessorId,
                ProfessorFullName = user.FirstName + " " + user.LastName,
                reqCourse.Priority
            }
        ).ToListAsync(cancellationToken);

        var demandDict = studentDemand.ToDictionary(x => x.CourseId, x => x.DemandCount);
        var profInterestsGrouped = (
            from p in professorInterests
            group p by p.CourseId into g
            select g
        ).ToDictionary(
            g => g.Key,
            g => (ICollection<ProfessorInterestDto>)(
                from x in g
                orderby x.Priority, x.ProfessorFullName
                select new ProfessorInterestDto { ProfessorId = x.ProfessorId, ProfessorFullName = x.ProfessorFullName, Priority = x.Priority }
            ).ToList()
        );

        var relevantCourseIds = demandDict.Keys.Union(profInterestsGrouped.Keys).ToArray();
        if (relevantCourseIds.Length == 0)
            return new AcademicPlanningOverviewDto { AcademicTermId = term.Id, AcademicTermCode = term.Code, AcademicTermTitle = term.Title, Courses = [] };

        var courses = await (
            from c in _context.Courses.AsNoTracking()
            where relevantCourseIds.Contains(c.Id)
            select new { c.Id, c.Code, c.Title, c.Credits }
        ).ToListAsync(cancellationToken);

        var overviewCourses = (
            from c in courses
            let demand = demandDict.GetValueOrDefault(c.Id, 0)
            let profs = profInterestsGrouped.GetValueOrDefault(c.Id, (ICollection<ProfessorInterestDto>)[])
            where demand > 0 || profs.Count > 0
            orderby demand descending, c.Code
            select new CoursePlanningOverviewDto
            {
                CourseId = c.Id,
                CourseCode = c.Code,
                CourseTitle = c.Title,
                Credits = c.Credits,
                StudentDemandCount = demand,
                InterestedProfessorsCount = profs.Count,
                InterestedProfessors = profs
            }
        ).ToList();

        return new AcademicPlanningOverviewDto
        {
            AcademicTermId = term.Id,
            AcademicTermCode = term.Code,
            AcademicTermTitle = term.Title,
            Courses = overviewCourses
        };
    }
}
