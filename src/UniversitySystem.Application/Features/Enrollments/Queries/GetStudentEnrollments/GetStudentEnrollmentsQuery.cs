using MediatR;
using UniversitySystem.Application.Features.Enrollments.DTOs;
using UniversitySystem.Application.Features.Enrollments.Services;

namespace UniversitySystem.Application.Features.Enrollments.Queries.GetStudentEnrollments;

/// <summary>
/// درخواست خواندن اطلاعات برای «مشاهده ثبت‌نام‌های دانشجو در ترم»؛ هدف آن دریافت پاسخ بدون تغییر داده است.
/// </summary>
public sealed record GetStudentEnrollmentsQuery(long AcademicTermId) : IRequest<ICollection<EnrollmentDto>>;
