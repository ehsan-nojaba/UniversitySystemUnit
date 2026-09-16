using System.Text.Json.Serialization;
using UniversitySystem.Api.Serialization;
using UniversitySystem.Application.Features.CourseOfferings.Commands.CreateCourseOffering;
using UniversitySystem.Application.Features.CourseOfferings.Commands.SaveCourseOfferingSchedule;
using UniversitySystem.Application.Features.CourseOfferings.Commands.UpdateCourseOffering;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Queries.GetCourseOfferingSchedule;
using UniversitySystem.Application.Features.CourseOfferings.Queries.GetCourseOfferings;
using UniversitySystem.Application.Features.TeachingAssignments.Commands.AssignProfessor;
using UniversitySystem.Application.Features.TeachingAssignments.Commands.RemoveProfessorAssignment;
using UniversitySystem.Application.Features.TeachingAssignments.DTOs;
using UniversitySystem.Application.Features.TeachingAssignments.Queries.GetTeachingAssignments;

namespace UniversitySystem.Api.Contracts;

/// <summary>
/// بدنه HTTP ذخیره برنامه ارائه؛ فهرست روز و بازه‌های کلاس را در Slots نگه می‌دارد.
/// </summary>
[JsonConverter(typeof(SaveCourseOfferingScheduleRequestConverter))]
public class SaveCourseOfferingScheduleRequest
{
    public List<CourseOfferingScheduleSlotDto> Slots { get; set; } = [];
}
