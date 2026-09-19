using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.CourseOfferings.Repositories;
using UniversitySystem.Application.Features.Enrollments.Repositories;
using UniversitySystem.Application.Features.StudentPreRegistration.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Features.StudentResults.DTOs;

/// <summary>
/// نتیجه یک درس درخواستی دانشجو: مشخصات درس، اولویت، داشتن ارائه فعال و جزئیات ارائه‌ها.
/// </summary>
public sealed record CourseResultDto(long CourseId, string Code, string Title, int Priority, bool IsOffered,
    ICollection<OfferingResultDto> Offerings);
