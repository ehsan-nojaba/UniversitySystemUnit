namespace UniversitySystem.Api.Contracts;

/// <summary>شناسه ارائه‌ای که دانشجو برای ثبت‌نام قطعی انتخاب کرده است.</summary>
public sealed record CreateEnrollmentRequest(long CourseOfferingId);
