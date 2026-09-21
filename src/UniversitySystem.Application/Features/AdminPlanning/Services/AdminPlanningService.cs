using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Features.AdminPlanning.DTOs;
using UniversitySystem.Application.Features.AdminPlanning.Repositories;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Features.AdminPlanning.Services;
/// <summary>
/// تقاضای پیش‌انتخاب ارسال‌شده و علاقه استادهای ارسال‌شده را برای هر درس کنار هم قرار می‌دهد و بر اساس تقاضا مرتب می‌کند؛ تصمیم ارائه را خودکار نمی‌گیرد.
/// </summary>
public sealed class AdminPlanningService(IAdminPlanningRepository repository) : IAdminPlanningService
{
    public async Task<AcademicPlanningOverviewDto> GetPlanningOverviewAsync(long academicTermId, CancellationToken cancellationToken = default)
    {
        if (academicTermId <= 0)
        {
            throw new BusinessException("شناسه ترم تحصیلی نامعتبر است.");
        }

        var term = await repository.GetAcademicTermInfoAsync(academicTermId, cancellationToken);
        if (term is null)
        {
            throw new NotFoundException(nameof(AcademicTerm), academicTermId);
        }

        var studentDemands = await repository.GetSubmittedStudentDemandCountsAsync(academicTermId, cancellationToken);
        var professorInterests = await repository.GetSubmittedProfessorInterestsAsync(academicTermId, cancellationToken);
        var demandDict = studentDemands.ToDictionary(x => x.CourseId, x => x.DemandCount);
        var profInterestsGrouped = ( from p in professorInterests group p by p.CourseId into g select g).ToDictionary(g => g.Key, g => (ICollection<ProfessorInterestDto>)( from x in g orderby x.Priority, x.ProfessorFullName select new ProfessorInterestDto { ProfessorId = x.ProfessorId, ProfessorFullName = x.ProfessorFullName, Priority = x.Priority }).ToList());
        var relevantCourseIds = demandDict.Keys.Union(profInterestsGrouped.Keys).ToArray();
        if (relevantCourseIds.Length == 0)
        {
            return new AcademicPlanningOverviewDto
            {
                AcademicTermId = term.Id,
                AcademicTermCode = term.Code,
                AcademicTermTitle = term.Title,
                Courses = []
            };
        }

        var courses = await repository.GetCoursesByIdsAsync(relevantCourseIds, cancellationToken);
        var overviewCourses = ( from c in courses let demand = demandDict.GetValueOrDefault(c.Id, 0)let profs = profInterestsGrouped.GetValueOrDefault(c.Id, (ICollection<ProfessorInterestDto>)[]) where demand > 0 || profs.Count > 0 orderby demand descending, c.Code select new CoursePlanningOverviewDto { CourseId = c.Id, CourseCode = c.Code, CourseTitle = c.Title, Credits = c.Credits, StudentDemandCount = demand, InterestedProfessorsCount = profs.Count, InterestedProfessors = profs }  ).ToList();
        return new AcademicPlanningOverviewDto
        {
            AcademicTermId = term.Id,
            AcademicTermCode = term.Code,
            AcademicTermTitle = term.Title,
            Courses = overviewCourses
        };
    }
}
