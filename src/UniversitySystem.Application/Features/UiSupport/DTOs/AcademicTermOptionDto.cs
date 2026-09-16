namespace UniversitySystem.Application.Features.UiSupport.DTOs;
/// <summary>گزینه انتخاب ترم در UI، همراه وضعیت فعالیت و تاریخ شروع و پایان.</summary>
public sealed record AcademicTermOptionDto(long Id, string Code, string Title, bool IsActive, DateTime StartDate, DateTime EndDate);
