namespace UniversitySystem.Application.Features.UiSupport.DTOs;
/// <summary>اطلاعات حساب جاری برای راه‌اندازی UI؛ اطلاعات حساس رمز در پاسخ وجود ندارد.</summary>
public sealed record CurrentUserDto(long Id, string Username, string FullName, IReadOnlyCollection<string> Roles, StudentProfileDto? Student, ProfessorOptionDto? Professor);
