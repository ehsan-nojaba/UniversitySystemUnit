namespace UniversitySystem.Application.Features.AdminPlanning.Repositories;

/// <summary>
/// خروجی سبک ریپازیتوری برنامه‌ریزی شامل شناسه، کد و عنوان ترم؛ موجودیت دیتابیس جدید نیست.
/// </summary>
public class AcademicTermInfoModel
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
}
