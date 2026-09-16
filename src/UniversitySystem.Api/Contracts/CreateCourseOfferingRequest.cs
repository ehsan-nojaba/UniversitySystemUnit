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
/// بدنه HTTP ایجاد ارائه؛ شناسه ترم و درس و ظرفیت را دریافت می‌کند.
/// </summary>
public class CreateCourseOfferingRequest
{
    public long AcademicTermId { get; set; }
    public long CourseId { get; set; }
    public int Capacity { get; set; }
}
