using MediatR;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Services;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.Commands.ReopenTeachingRequest;

public sealed class ReopenTeachingRequestCommandHandler(ProfessorTeachingRequestService service) : IRequestHandler<ReopenTeachingRequestCommand, TeachingRequestDto>
{
    public Task<TeachingRequestDto> Handle(ReopenTeachingRequestCommand request, CancellationToken cancellationToken)
    {
        return service.ReopenAsync(request.AcademicTermId, cancellationToken);
    }
}
