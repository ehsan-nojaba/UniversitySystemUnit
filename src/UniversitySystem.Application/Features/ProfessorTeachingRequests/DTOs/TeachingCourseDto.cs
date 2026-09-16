using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;

/// <summary>
/// مشخصات نمایشی درس پیشنهادی استاد: شناسه، کد، عنوان، تعداد واحد و اولویت تدریس.
/// </summary>
public sealed record TeachingCourseDto(long CourseId, string Code, string Title, int Credits, int Priority);
