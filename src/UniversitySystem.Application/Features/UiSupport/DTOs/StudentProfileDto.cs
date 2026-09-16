namespace UniversitySystem.Application.Features.UiSupport.DTOs;
/// <summary>پروفایل نمایشی دانشجو همراه شماره دانشجویی، رشته و سال ورود.</summary>
public sealed record StudentProfileDto(long Id, string StudentNumber, long MajorId, string MajorTitle, int EntryYear);
