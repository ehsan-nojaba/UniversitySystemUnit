using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using UniversitySystem.Api.Controllers;
using UniversitySystem.Application.Features.AdminPlanning.DTOs;
using UniversitySystem.Application.Features.AdminPreRegistration.DTOs;
using UniversitySystem.Application.Features.Auth.Commands.Login;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Application.Features.TeachingAssignments.DTOs;

namespace UniversitySystem.Api.Infrastructure;

/// <summary>
/// مشخصات پاسخ مسیرهای API را متمرکز ثبت می‌کند تا کنترلرها فقط مسئول HTTP باشند.
/// این Convention به ApiExplorer و Scalar اطلاعات می‌دهد و رفتار مدیریت خطا را تغییر نمی‌دهد.
/// </summary>
public sealed class ApiResponseConvention : IActionModelConvention
{
    public void Apply(ActionModel action)
    {
        ProducesResponseTypeAttribute[] responses = (action.Controller.ControllerType.Name, action.ActionName) switch
        {
            (nameof(AdminCourseOfferingsController), nameof(AdminCourseOfferingsController.GetByTerm)) => Responses(typeof(ICollection<CourseOfferingDto>), StatusCodes.Status200OK, StatusCodes.Status400BadRequest, StatusCodes.Status401Unauthorized, StatusCodes.Status403Forbidden, StatusCodes.Status404NotFound),
            (nameof(AdminCourseOfferingsController), nameof(AdminCourseOfferingsController.Create)) => Responses(typeof(CourseOfferingDto), StatusCodes.Status201Created, StatusCodes.Status400BadRequest, StatusCodes.Status401Unauthorized, StatusCodes.Status403Forbidden, StatusCodes.Status404NotFound),
            (nameof(AdminCourseOfferingsController), nameof(AdminCourseOfferingsController.Update)) => Responses(typeof(CourseOfferingDto), StatusCodes.Status200OK, StatusCodes.Status400BadRequest, StatusCodes.Status401Unauthorized, StatusCodes.Status403Forbidden, StatusCodes.Status404NotFound),
            (nameof(AdminCourseOfferingsController), nameof(AdminCourseOfferingsController.GetProfessors)) => Responses(typeof(ICollection<TeachingAssignmentDto>), StatusCodes.Status200OK, StatusCodes.Status400BadRequest, StatusCodes.Status401Unauthorized, StatusCodes.Status403Forbidden, StatusCodes.Status404NotFound),
            (nameof(AdminCourseOfferingsController), nameof(AdminCourseOfferingsController.AssignProfessor)) => Responses(typeof(TeachingAssignmentDto), StatusCodes.Status201Created, StatusCodes.Status400BadRequest, StatusCodes.Status401Unauthorized, StatusCodes.Status403Forbidden, StatusCodes.Status404NotFound),
            (nameof(AdminCourseOfferingsController), nameof(AdminCourseOfferingsController.RemoveProfessor)) => Responses(null, StatusCodes.Status204NoContent, StatusCodes.Status400BadRequest, StatusCodes.Status401Unauthorized, StatusCodes.Status403Forbidden, StatusCodes.Status404NotFound),
            (nameof(AdminCourseOfferingsController), nameof(AdminCourseOfferingsController.GetSchedule)) => Responses(typeof(ICollection<CourseOfferingScheduleDto>), StatusCodes.Status200OK, StatusCodes.Status400BadRequest, StatusCodes.Status401Unauthorized, StatusCodes.Status403Forbidden, StatusCodes.Status404NotFound),
            (nameof(AdminCourseOfferingsController), nameof(AdminCourseOfferingsController.SaveSchedule)) => Responses(typeof(ICollection<CourseOfferingScheduleDto>), StatusCodes.Status200OK, StatusCodes.Status400BadRequest, StatusCodes.Status401Unauthorized, StatusCodes.Status403Forbidden, StatusCodes.Status404NotFound),
            (nameof(AdminPlanningController), nameof(AdminPlanningController.GetOverview)) => Responses(typeof(AcademicPlanningOverviewDto), StatusCodes.Status200OK, StatusCodes.Status400BadRequest, StatusCodes.Status401Unauthorized, StatusCodes.Status403Forbidden, StatusCodes.Status404NotFound),
            (nameof(AdminPreRegistrationController), nameof(AdminPreRegistrationController.GetDemandSummary)) => Responses(typeof(ICollection<CourseDemandDto>), StatusCodes.Status200OK, StatusCodes.Status400BadRequest, StatusCodes.Status401Unauthorized, StatusCodes.Status403Forbidden, StatusCodes.Status404NotFound),
            (nameof(AuthController), nameof(AuthController.Login)) => Responses(typeof(LoginResponse), StatusCodes.Status200OK, StatusCodes.Status400BadRequest, StatusCodes.Status401Unauthorized),
            (nameof(StudentPreRegistrationController), nameof(StudentPreRegistrationController.GetEligibleCourses)) => Responses(typeof(ICollection<EligibleCourseDto>), StatusCodes.Status200OK, StatusCodes.Status400BadRequest, StatusCodes.Status401Unauthorized, StatusCodes.Status403Forbidden, StatusCodes.Status404NotFound),
            (nameof(StudentPreRegistrationController), nameof(StudentPreRegistrationController.GetPreRegistration)) => Responses(typeof(StudentPreRegistrationDto), StatusCodes.Status200OK, StatusCodes.Status401Unauthorized, StatusCodes.Status403Forbidden, StatusCodes.Status404NotFound),
            (nameof(StudentPreRegistrationController), nameof(StudentPreRegistrationController.SavePreRegistration)) => Responses(typeof(StudentPreRegistrationDto), StatusCodes.Status200OK, StatusCodes.Status400BadRequest, StatusCodes.Status401Unauthorized, StatusCodes.Status403Forbidden, StatusCodes.Status404NotFound),
            (nameof(StudentPreRegistrationController), nameof(StudentPreRegistrationController.SubmitPreRegistration)) => Responses(typeof(StudentPreRegistrationDto), StatusCodes.Status200OK, StatusCodes.Status400BadRequest, StatusCodes.Status401Unauthorized, StatusCodes.Status403Forbidden, StatusCodes.Status404NotFound),
            _ => []
        };

        var declaredResponses = action.Attributes.OfType<SwaggerResponseAttribute>().ToArray();
        foreach (var declared in declaredResponses)
        {
            action.Filters.Add(new ProducesResponseTypeAttribute(declared.Type ?? typeof(void), declared.StatusCode));
        }
        foreach (var response in responses.Where(response => !declaredResponses.Any(declared => declared.StatusCode == response.StatusCode)))
        {
            action.Filters.Add(response);
        }
    }
    private static ProducesResponseTypeAttribute[] Responses(Type? responseType, int successStatus, params int[] errorStatuses)
    {
        var success = responseType is null ? new ProducesResponseTypeAttribute(successStatus) : new ProducesResponseTypeAttribute(responseType, successStatus);
        return [success, .. errorStatuses.Select(status => new ProducesResponseTypeAttribute(typeof(ProblemDetails), status))];
    }
}

