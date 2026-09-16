namespace UniversitySystem.Application.Features.CourseOfferings.DTOs;

/// <summary>
/// پاسخ مشخصات ارائه: شناسه ارائه، ترم، مشخصات درس، ظرفیت و وضعیت فعالیت.
/// </summary>
public class CourseOfferingDto
{
    public long CourseOfferingId { get; set; }
    public long AcademicTermId { get; set; }
    public long CourseId { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public bool IsActive { get; set; }
}
