using Microsoft.AspNetCore.Mvc;

namespace UniversitySystem.Api.Infrastructure;

/// <summary>تبدیل نتیجه سرویس به پاسخ HTTP، بدون تغییر قالب JSON و بدون دخالت در منطق آموزشی.</summary>
public static class ApiResponseExtensions
{
    public static IActionResult ToApiResponse(this object? response, int statusCode = StatusCodes.Status200OK)
    {
        if (statusCode == StatusCodes.Status204NoContent)
        {
            return new NoContentResult();
        }
        if (response is null)
        {
            return new NotFoundObjectResult(new ProblemDetails { Status = StatusCodes.Status404NotFound, Title = "اطلاعات پیدا نشد" });
        }
        return new ObjectResult(response) { StatusCode = statusCode };
    }

    public static IActionResult ToApiResponse(this object response, string actionName, object routeValues)
    {
        return new CreatedAtActionResult(actionName, null, routeValues, response);
    }
}
