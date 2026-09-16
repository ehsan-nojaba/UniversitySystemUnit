using MediatR;
using UniversitySystem.Application.Features.StudentResults.DTOs;

namespace UniversitySystem.Application.Features.StudentResults.Queries.GetStudentResult;

/// <summary>
/// درخواست خواندن اطلاعات برای «دریافت نتیجه ارائه درس‌های پیش‌انتخاب»؛ هدف آن دریافت پاسخ بدون تغییر داده است.
/// </summary>
public sealed record GetStudentResultQuery(long AcademicTermId) : IRequest<StudentResultDto>;
