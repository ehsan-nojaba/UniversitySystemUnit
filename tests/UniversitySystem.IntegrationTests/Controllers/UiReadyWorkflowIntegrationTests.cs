using System.Text.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.Auth.Commands.Login;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.Enrollments.DTOs;
using UniversitySystem.Application.Features.UiSupport.DTOs;
using UniversitySystem.Domain.Constants;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Persistence.Data;
using UniversitySystem.Application.Common.Logic;

namespace UniversitySystem.IntegrationTests.Controllers;
public sealed class UiReadyWorkflowIntegrationTests : IClassFixture<UniversityApiFactory>
{
    private readonly UniversityApiFactory factory;
    public UiReadyWorkflowIntegrationTests(UniversityApiFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task Student_Professor_Admin_Workflow_Is_Ready_For_Ui()
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var term = AcademicTermLogic.Create("T" + suffix,"ترم نمونه",DateTime.UtcNow,DateTime.UtcNow.AddMonths(4));
        var faculty = FacultyLogic.Create("F" + suffix,"دانشکده");
        db.AddRange(term, faculty);
        await db.SaveChangesAsync();
        var department = DepartmentLogic.Create(faculty.Id,"D" + suffix,"گروه");
        db.Add(department);
        await db.SaveChangesAsync();
        var major = MajorLogic.Create(department.Id,"M" + suffix,"مهندسی کامپیوتر");
        var course = CourseLogic.Create("C" + suffix,"پایگاه داده",3);
        var inactive = CourseLogic.Create("I" + suffix,"غیرفعال",3);
        CourseLogic.Deactivate(        inactive);
        var studentUser = UserLogic.Create("s" + suffix,hasher.Hash("TestPassword123!"),"دانشجو","نمونه");
        var professorUser = UserLogic.Create("p" + suffix,hasher.Hash("TestPassword123!"),"استاد","نمونه");
        var adminUser = UserLogic.Create("a" + suffix,hasher.Hash("TestPassword123!"),"آموزش","نمونه");
        db.AddRange(major, course, inactive, studentUser, professorUser, adminUser);
        await db.SaveChangesAsync();
        var student = StudentLogic.Create(studentUser.Id,"S" + suffix,major.Id,1403);
        var professor = ProfessorLogic.Create(professorUser.Id,"P" + suffix);
        var curriculum = CurriculumLogic.Create(major.Id,"چارت",suffix);
        CurriculumLogic.AddCourse(        curriculum,course.Id,1,true);
        db.AddRange(student, professor, curriculum);
        await db.SaveChangesAsync();
        foreach (var pair in new[]
        {
            (studentUser, RoleNames.Student),
            (professorUser, RoleNames.Professor),
            (adminUser, RoleNames.EducationAdmin)
        }

        )
        {
            var role = db.Roles.FirstOrDefault(r => r.Name == pair.Item2);
            if (role is null)
            {
                role = RoleLogic.Create(pair.Item2);
                db.Add(role);
                await db.SaveChangesAsync();
            }

            UserLogic.AssignRole(pair.Item1, role);
        }

        await db.SaveChangesAsync();
        using var studentClient = await LoginAsync(studentUser.Username);
        using var professorClient = await LoginAsync(professorUser.Username);
        using var adminClient = await LoginAsync(adminUser.Username);
        var me = await studentClient.GetFromJsonAsync<CurrentUserDto>("/api/v1/auth/me");
        Assert.Equal(student.Id, me!.Student!.Id);
        Assert.Contains(RoleNames.Student, me.Roles);
        Assert.Equal(HttpStatusCode.Forbidden, (await studentClient.GetAsync("/api/v1/lookups/professors")).StatusCode);
        var courses = await professorClient.GetFromJsonAsync<List<CourseOptionDto>>("/api/v1/lookups/courses");
        Assert.Contains(courses!, c => c.Id == course.Id);
        Assert.DoesNotContain(courses!, c => c.Id == inactive.Id);
        var terms = await studentClient.GetFromJsonAsync<List<AcademicTermOptionDto>>("/api/v1/lookups/academic-terms");
        Assert.Contains(terms!, t => t.Id == term.Id);
        var professors = await adminClient.GetFromJsonAsync<List<ProfessorOptionDto>>("/api/v1/lookups/professors");
        Assert.Contains(professors!, p => p.Id == professor.Id);
        var studentPath = $"/api/v1/student/pre-registration/{term.Id}";
        Assert.Equal(HttpStatusCode.OK, (await studentClient.PutAsJsonAsync(studentPath, new { courses = new[] { new { courseId = course.Id, priority = 1 } } })).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await studentClient.PostAsync(studentPath + "/submit", null)).StatusCode);
        var allowedCourses = await adminClient.PutAsJsonAsync($"/api/v1/admin/professors/{professor.Id}/courses", new { courseIds = new[] { course.Id } });
        Assert.Equal(HttpStatusCode.OK, allowedCourses.StatusCode);
        var professorPath = $"/api/v1/professor/teaching-request/{term.Id}";
        Assert.Equal(HttpStatusCode.OK, (await professorClient.PutAsJsonAsync(professorPath, new { courses = new[] { new { courseId = course.Id, priority = 1 } } })).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await professorClient.PutAsJsonAsync(professorPath + "/availability", new { availability = new[] { new { courseId = course.Id, dayOfWeek = 6, startTime = "08:00:00", endTime = "12:00:00" } } })).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await professorClient.PostAsync(professorPath + "/submit", null)).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await professorClient.PutAsJsonAsync(professorPath, new { courses = new[] { new { courseId = course.Id, priority = 2 } } })).StatusCode);
        var created = await adminClient.PostAsJsonAsync("/api/v1/admin/course-offerings", new { academicTermId = term.Id, courseId = course.Id, capacity = 1 });
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);
        var offering = (await created.Content.ReadFromJsonAsync<CourseOfferingDto>())!;
        var offeringPath = $"/api/v1/admin/course-offerings/{offering.CourseOfferingId}";
        Assert.Equal(HttpStatusCode.Created, (await adminClient.PostAsJsonAsync(offeringPath + "/professors", new { professorId = professor.Id })).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await adminClient.PutAsJsonAsync(offeringPath + "/schedule", new { slots = new[] { new { dayOfWeek = 6, startTime = "08:00:00", endTime = "10:00:00" } } })).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await adminClient.PostAsync(offeringPath + "/finalize", null)).StatusCode);
        var resultResponse = await studentClient.GetAsync(studentPath + "/result");
        Assert.Equal(HttpStatusCode.OK, resultResponse.StatusCode);
        using var result = JsonDocument.Parse(await resultResponse.Content.ReadAsStringAsync());
        var resultOffering = result.RootElement.GetProperty("courses")[0].GetProperty("offerings")[0];
        Assert.Equal(1, resultOffering.GetProperty("remainingCapacity").GetInt32());
        Assert.Equal(professor.Id, resultOffering.GetProperty("professors")[0].GetProperty("professorId").GetInt64());
        Assert.Equal(1, resultOffering.GetProperty("schedule").GetArrayLength());
        var enrollmentResponse = await studentClient.PostAsJsonAsync("/api/v1/student/enrollments", new { courseOfferingId = offering.CourseOfferingId });
        Assert.Equal(HttpStatusCode.Created, enrollmentResponse.StatusCode);
        var enrollment = (await enrollmentResponse.Content.ReadFromJsonAsync<EnrollmentDto>())!;
        Assert.Equal(course.Id, enrollment.CourseId);
        Assert.Equal(HttpStatusCode.BadRequest, (await studentClient.PostAsJsonAsync("/api/v1/student/enrollments", new { courseOfferingId = offering.CourseOfferingId })).StatusCode);
        var enrollments = await studentClient.GetFromJsonAsync<List<EnrollmentDto>>($"/api/v1/student/enrollments?academicTermId={term.Id}");
        Assert.Single(enrollments!);
        Assert.Equal(HttpStatusCode.OK, (await adminClient.GetAsync($"/api/v1/admin/reports?academicTermId={term.Id}")).StatusCode);
    }

    [Fact]
    public async Task Cors_Only_Allows_Configured_Local_Ui_Origin()
    {
        using var client = factory.CreateClient();
        foreach (var origin in new[]
        {
            "http://localhost:5173",
            "https://untrusted.example"
        }

        )
        {
            using var request = new HttpRequestMessage(HttpMethod.Options, "/api/v1/auth/login");
            request.Headers.Add("Origin", origin);
            request.Headers.Add("Access-Control-Request-Method", "POST");
            request.Headers.Add("Access-Control-Request-Headers", "content-type");
            var response = await client.SendAsync(request);
            Assert.Equal(origin == "http://localhost:5173", response.Headers.Contains("Access-Control-Allow-Origin"));
        }
    }

    [Fact]
    public async Task Scalar_And_OpenApi_Expose_Ui_Endpoints_And_Bearer_Authentication()
    {
        using var developmentFactory = new UniversityApiFactory { EnvironmentName = "Development" };
        using var client = developmentFactory.CreateClient(new() { BaseAddress = new Uri("https://localhost") });
        var page = await client.GetStringAsync("/scalar/v1");
        Assert.Contains("scalar", page, StringComparison.OrdinalIgnoreCase);
        using var document = JsonDocument.Parse(await client.GetStringAsync("/openapi/v1.json"));
        var paths = document.RootElement.GetProperty("paths");
        Assert.True(paths.TryGetProperty("/api/v1/auth/me", out _));
        Assert.True(paths.TryGetProperty("/api/v1/lookups/academic-terms", out _));
        Assert.True(paths.TryGetProperty("/api/v1/lookups/courses", out _));
        Assert.True(paths.TryGetProperty("/api/v1/lookups/professors", out _));
        Assert.Equal("bearer", document.RootElement.GetProperty("components").GetProperty("securitySchemes").GetProperty("Bearer").GetProperty("scheme").GetString());
        Assert.Equal("Bearer", paths.GetProperty("/api/v1/auth/me").GetProperty("get").GetProperty("security")[0].EnumerateObject().First().Name);
        var loginPath = paths.EnumerateObject().Single(path => path.Name.Equals("/api/v1/auth/login", StringComparison.OrdinalIgnoreCase));
        Assert.False(loginPath.Value.GetProperty("post").TryGetProperty("security", out _));
        Assert.Equal("ورود به سامانه", loginPath.Value.GetProperty("post").GetProperty("summary").GetString());
        var loginResponses = loginPath.Value.GetProperty("post").GetProperty("responses");
        Assert.True(loginResponses.TryGetProperty("200", out _));
        Assert.True(loginResponses.TryGetProperty("400", out _));
        Assert.True(loginResponses.TryGetProperty("401", out _));
        Assert.Contains("LoginResponse", loginResponses.GetProperty("200").GetProperty("content").GetProperty("application/json").GetProperty("schema").GetProperty("$ref").GetString());
        var createResponses = paths.GetProperty("/api/v1/admin/course-offerings").GetProperty("post").GetProperty("responses");
        Assert.True(createResponses.TryGetProperty("201", out _));
        Assert.False(createResponses.TryGetProperty("200", out _));
    }

    [Fact]
    public async Task Current_User_Rejects_Anonymous_And_Deactivated_Account()
    {
        using var client = factory.CreateClient();
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/v1/auth/me")).StatusCode);
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var user = UserLogic.Create("inactive" + Guid.NewGuid().ToString("N"),"unused","Inactive","User");
        UserLogic.Deactivate(        user);
        db.Add(user);
        await db.SaveChangesAsync();
        var token = scope.ServiceProvider.GetRequiredService<ITokenService>().GenerateToken(user, [RoleNames.Student]).Item1;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/v1/auth/me")).StatusCode);
    }

    private async Task<HttpClient> LoginAsync(string username)
    {
        var client = factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/v1/auth/login", new { username, password = "TestPassword123!" });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var login = (await response.Content.ReadFromJsonAsync<LoginResponse>())!;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.AccessToken);
        return client;
    }
}

