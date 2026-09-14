namespace UniversitySystem.Application.Features.AdminPlanning.Repositories;

public class AcademicTermInfoModel
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
}

public class CourseDemandCountModel
{
    public long CourseId { get; set; }
    public int DemandCount { get; set; }
}

public class ProfessorCourseInterestModel
{
    public long CourseId { get; set; }
    public long ProfessorId { get; set; }
    public string ProfessorFullName { get; set; } = string.Empty;
    public int Priority { get; set; }
}

public class CourseInfoModel
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }
}
