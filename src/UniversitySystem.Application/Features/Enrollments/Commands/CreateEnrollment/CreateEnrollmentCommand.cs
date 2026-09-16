using MediatR;
using UniversitySystem.Application.Features.Enrollments.DTOs;
using UniversitySystem.Application.Features.Enrollments.Services;

namespace UniversitySystem.Application.Features.Enrollments.Commands.CreateEnrollment;

/// <summary>
/// درخواست انجام عملیات «ثبت‌نام قطعی دانشجو در ارائه»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود.
/// </summary>
public sealed record CreateEnrollmentCommand(long CourseOfferingId) : IRequest<EnrollmentDto>;
