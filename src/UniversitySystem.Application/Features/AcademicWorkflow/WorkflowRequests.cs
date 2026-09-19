using MediatR;

namespace UniversitySystem.Application.Features.AcademicWorkflow;

/// <summary>پیام‌های مشاهده ارتباط استاد و درس، ذخیره ارتباط و نهایی‌کردن برنامه؛ فاقد کوئری دیتابیس هستند.</summary>
public sealed record MyTeachingCoursesQuery(long AcademicTermId) : IRequest<ICollection<ProfessorCourseOption>>;
public sealed record MyFinalScheduleQuery(long AcademicTermId) : IRequest<ICollection<FinalTeachingOffering>>;
public sealed record ProfessorCoursesQuery(long ProfessorId) : IRequest<ICollection<long>>;
public sealed record SaveProfessorCoursesCommand(long ProfessorId, ICollection<long> CourseIds) : IRequest<ICollection<long>>;
public sealed record FinalizeOfferingCommand(long CourseOfferingId, bool Finalize) : IRequest<bool>;

/// <summary>درخواست‌های فرایند آموزشی را به سرویس Application می‌سپارد.</summary>
public sealed class WorkflowRequestHandler(AcademicWorkflowService service) : IRequestHandler<MyFinalScheduleQuery, ICollection<FinalTeachingOffering>>, IRequestHandler<MyTeachingCoursesQuery, ICollection<ProfessorCourseOption>>, IRequestHandler<ProfessorCoursesQuery, ICollection<long>>, IRequestHandler<SaveProfessorCoursesCommand, ICollection<long>>, IRequestHandler<FinalizeOfferingCommand, bool>
{
    public Task<ICollection<FinalTeachingOffering>> Handle(MyFinalScheduleQuery r, CancellationToken ct) => service.GetMyFinalScheduleAsync(r.AcademicTermId, ct);
    public Task<ICollection<ProfessorCourseOption>> Handle(MyTeachingCoursesQuery r, CancellationToken ct) => service.GetMyCoursesAsync(r.AcademicTermId, ct);
    public Task<ICollection<long>> Handle(ProfessorCoursesQuery r, CancellationToken ct) => service.GetAllowedAsync(r.ProfessorId, ct);
    public Task<ICollection<long>> Handle(SaveProfessorCoursesCommand r, CancellationToken ct) => service.SaveAllowedAsync(r.ProfessorId, r.CourseIds, ct);
    public Task<bool> Handle(FinalizeOfferingCommand r, CancellationToken ct) => service.SetFinalizedAsync(r.CourseOfferingId, r.Finalize, ct);
}
