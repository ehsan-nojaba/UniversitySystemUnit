using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;

/// <summary>
/// خلاصه مخصوص آموزش؛ شناسه و نام استاد را همراه جزئیات درخواست ارسال‌شده برمی‌گرداند.
/// </summary>
public sealed record TeachingRequestSummaryDto(long ProfessorId, string ProfessorFullName, TeachingRequestDto Request);
