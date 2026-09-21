using MediatR;

namespace UniversitySystem.Application.Features.MajorCurricula;

/// <summary>پیام‌های مدیریت چارت رشته؛ شناسه رشته فقط از مسیر کنترلر آموزش گرفته می‌شود.</summary>
public sealed record GetMajorsQuery : IRequest<ICollection<MajorOptionDto>>;
public sealed record GetMajorCurriculumQuery(long MajorId) : IRequest<MajorCurriculumDto>;
public sealed record GetNextCourseCodeQuery(long MajorId) : IRequest<NextCourseCodeDto>;
public sealed record SaveMajorCurriculumCommand(long MajorId, ICollection<CurriculumCourseInput> Courses) : IRequest<MajorCurriculumDto>;
public sealed record CreateMajorCourseCommand(long MajorId, string Title, int Credits, int RecommendedTerm, bool IsRequired) : IRequest<MajorCurriculumDto>;

/// <summary>عملیات چارت را بدون دسترسی مستقیم به دیتابیس به سرویس مربوط می‌سپارد.</summary>
public sealed class CurriculumRequestHandler(MajorCurriculumService service) : IRequestHandler<GetMajorsQuery, ICollection<MajorOptionDto>>, IRequestHandler<GetMajorCurriculumQuery, MajorCurriculumDto>, IRequestHandler<GetNextCourseCodeQuery, NextCourseCodeDto>, IRequestHandler<SaveMajorCurriculumCommand, MajorCurriculumDto>, IRequestHandler<CreateMajorCourseCommand, MajorCurriculumDto>
{
    public Task<ICollection<MajorOptionDto>> Handle(GetMajorsQuery r, CancellationToken ct) => service.GetMajorsAsync(ct);
    public Task<MajorCurriculumDto> Handle(GetMajorCurriculumQuery r, CancellationToken ct) => service.GetAsync(r.MajorId, ct);
    public Task<NextCourseCodeDto> Handle(GetNextCourseCodeQuery r, CancellationToken ct) => service.GetNextCourseCodeAsync(r.MajorId, ct);
    public Task<MajorCurriculumDto> Handle(SaveMajorCurriculumCommand r, CancellationToken ct) => service.SaveAsync(r.MajorId, r.Courses, ct);
    public Task<MajorCurriculumDto> Handle(CreateMajorCourseCommand r, CancellationToken ct) => service.CreateCourseAsync(r.MajorId, new(r.Title, r.Credits, r.RecommendedTerm, r.IsRequired), ct);
}
