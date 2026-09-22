using MediatR;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.Commands.ReopenTeachingRequest;

/// <summary>
/// درخواست بازکردن درخواست ارسال‌شده استاد برای افزودن یا ویرایش درس و زمان پیشنهادی.
/// </summary>
public class ReopenTeachingRequestCommand : IRequest<TeachingRequestDto>
{
    public long AcademicTermId { get; }

    public ReopenTeachingRequestCommand(long academicTermId)
    {
        AcademicTermId = academicTermId;
    }
}
