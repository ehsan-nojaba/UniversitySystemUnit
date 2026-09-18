using MediatR;

namespace UniversitySystem.Application.Features.AcademicWorkflow;

/// <summary>پیام‌های مشاهده ارتباط استاد و درس، ذخیره ارتباط و نهایی‌کردن برنامه؛ فاقد کوئری دیتابیس هستند.</summary>
public sealed record MyTeachingCoursesQuery(long AcademicTermId) : IRequest<IReadOnlyCollection<ProfessorCourseOption>>;
public sealed record MyFinalScheduleQuery(long AcademicTermId) : IRequest<IReadOnlyCollection<FinalTeachingOffering>>;
public sealed record ProfessorCoursesQuery(long ProfessorId) : IRequest<IReadOnlyCollection<long>>;
public sealed record SaveProfessorCoursesCommand(long ProfessorId, IReadOnlyCollection<long> CourseIds) : IRequest<IReadOnlyCollection<long>>;
public sealed record FinalizeOfferingCommand(long CourseOfferingId, bool Finalize) : IRequest<bool>;

/// <summary>درخواست‌های فرایند آموزشی را به سرویس Application می‌سپارد.</summary>
public sealed class WorkflowRequestHandler(AcademicWorkflowService service) : IRequestHandler<MyFinalScheduleQuery, IReadOnlyCollection<FinalTeachingOffering>>, IRequestHandler<MyTeachingCoursesQuery, IReadOnlyCollection<ProfessorCourseOption>>, IRequestHandler<ProfessorCoursesQuery, IReadOnlyCollection<long>>, IRequestHandler<SaveProfessorCoursesCommand, IReadOnlyCollection<long>>, IRequestHandler<FinalizeOfferingCommand, bool>
{
    public Task<IReadOnlyCollection<FinalTeachingOffering>> Handle(MyFinalScheduleQuery r, CancellationToken ct) => service.GetMyFinalScheduleAsync(r.AcademicTermId, ct);
    public Task<IReadOnlyCollection<ProfessorCourseOption>> Handle(MyTeachingCoursesQuery r, CancellationToken ct) => service.GetMyCoursesAsync(r.AcademicTermId, ct);
    public Task<IReadOnlyCollection<long>> Handle(ProfessorCoursesQuery r, CancellationToken ct) => service.GetAllowedAsync(r.ProfessorId, ct);
    public Task<IReadOnlyCollection<long>> Handle(SaveProfessorCoursesCommand r, CancellationToken ct) => service.SaveAllowedAsync(r.ProfessorId, r.CourseIds, ct);
    public Task<bool> Handle(FinalizeOfferingCommand r, CancellationToken ct) => service.SetFinalizedAsync(r.CourseOfferingId, r.Finalize, ct);
}
