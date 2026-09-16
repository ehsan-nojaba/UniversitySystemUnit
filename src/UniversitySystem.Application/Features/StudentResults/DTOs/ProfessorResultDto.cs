using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.CourseOfferings.Repositories;
using UniversitySystem.Application.Features.Enrollments.Repositories;
using UniversitySystem.Application.Features.StudentPreRegistration.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Features.StudentResults.DTOs;

/// <summary>
/// شناسه و نام استاد تخصیص‌یافته به ارائه درس در نتیجه پیش‌انتخاب دانشجو.
/// </summary>
public sealed record ProfessorResultDto(long ProfessorId, string FullName);
