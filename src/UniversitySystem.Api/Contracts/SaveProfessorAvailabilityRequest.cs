using UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;

namespace UniversitySystem.Api.Contracts;

/// <summary>
/// بدنه HTTP ثبت بازه‌های آزاد استاد؛ روز هفته و زمان شروع و پایان هر بازه را دریافت می‌کند.
/// </summary>
public sealed record SaveProfessorAvailabilityRequest(ICollection<AvailabilityInput> Availability);
