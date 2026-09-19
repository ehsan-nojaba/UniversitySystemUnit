using MediatR;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.Commands.SaveTeachingRequest;

/// <summary>
/// درخواست انجام عملیات «ذخیره یا ویرایش درس‌های درخواست تدریس»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود.
/// </summary>
public sealed record SaveTeachingRequestCommand(long AcademicTermId, ICollection<TeachingCourseInput> Courses) : IRequest<TeachingRequestDto>;
