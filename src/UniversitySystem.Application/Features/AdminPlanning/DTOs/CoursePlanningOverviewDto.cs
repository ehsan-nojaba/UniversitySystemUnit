namespace UniversitySystem.Application.Features.AdminPlanning.DTOs;

/// <summary>
/// یک درس در نمای برنامه‌ریزی: واحد، تعداد تقاضای دانشجو و فهرست استادهای علاقه‌مند همراه اولویت.
/// </summary>
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
