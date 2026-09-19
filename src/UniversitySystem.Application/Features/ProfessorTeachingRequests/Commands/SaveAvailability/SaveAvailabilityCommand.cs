using MediatR;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.Commands.SaveAvailability;

/// <summary>
/// درخواست انجام عملیات «ثبت زمان‌های آزاد استاد»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود.
/// </summary>
public sealed record SaveAvailabilityCommand(long AcademicTermId, ICollection<AvailabilityInput> Availability) : IRequest<TeachingRequestDto>;
