namespace UniversitySystem.Application.Features.AdminPlanning.DTOs;

public class AcademicPlanningOverviewDto
{
    public long AcademicTermId { get; set; }
    public string AcademicTermCode { get; set; } = string.Empty;
    public string AcademicTermTitle { get; set; } = string.Empty;
    public ICollection<CoursePlanningOverviewDto> Courses { get; set; } = [];
}

public class CoursePlanningOverviewDto
{
    public long CourseId { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int StudentDemandCount { get; set; }
    public int InterestedProfessorsCount { get; set; }
    public ICollection<ProfessorInterestDto> InterestedProfessors { get; set; } = [];
}

public class ProfessorInterestDto
{
    public long ProfessorId { get; set; }
    public string ProfessorFullName { get; set; } = string.Empty;
    public int Priority { get; set; }
}
