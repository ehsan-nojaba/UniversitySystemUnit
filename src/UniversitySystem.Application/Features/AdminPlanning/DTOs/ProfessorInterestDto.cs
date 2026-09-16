namespace UniversitySystem.Application.Features.AdminPlanning.DTOs;

/// <summary>
/// شناسه، نام و اولویت استاد علاقه‌مند به درس؛ از درخواست ارسال‌شده استخراج می‌شود.
/// </summary>
public class ProfessorInterestDto
{
    public long ProfessorId { get; set; }
    public string ProfessorFullName { get; set; } = string.Empty;
    public int Priority { get; set; }
}
