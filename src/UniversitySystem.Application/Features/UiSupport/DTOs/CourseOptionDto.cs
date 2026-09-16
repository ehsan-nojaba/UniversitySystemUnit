namespace UniversitySystem.Application.Features.UiSupport.DTOs;
/// <summary>مشخصات درس فعال برای فرم درخواست استاد و ایجاد ارائه آموزش؛ مجاز بودن درس دانشجو API جدا دارد.</summary>
public sealed record CourseOptionDto(long Id, string Code, string Title, int Credits);
