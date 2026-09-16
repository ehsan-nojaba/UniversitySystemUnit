using MediatR;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.Commands.SubmitTeachingRequest;

/// <summary>
/// درخواست انجام عملیات «ارسال نهایی درخواست تدریس»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود.
/// </summary>
public sealed record SubmitTeachingRequestCommand(long AcademicTermId) : IRequest<TeachingRequestDto>;
