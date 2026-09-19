using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Persistence.Data;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetEligibleCourses;
using UniversitySystem.Domain.Constants;
using UniversitySystem.Domain.Entities;
using Xunit;
usingUniversitySystem.Application.Common.Logic;

namespace UniversitySystem.IntegrationTests.Controllers;

public class StudentEligibleCoursesIntegrationTests : IClassFixture<UniversityApiFactory>
{
    private readonly UniversityApiFactory _factory;
    private readonly HttpClient _client;
    private readonly ITokenService _tokenService;

    public StudentEligibleCoursesIntegrationTests(UniversityApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();

        using var scope = factory.Services.CreateScope();
        _tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
    }

    private string GenerateToken(User user, string role)
    {
        var (token, _) = _tokenService.GenerateToken(user, new[] { role });
        return token;
    }

    [Fact]
    public async Task GetEligibleCourses_WithoutToken_ReturnsUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/student/pre-registration/eligible-courses?academicTermId=1");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetEligibleCourses_WithProfessorRole_ReturnsForbidden()
    {
        // Arrange
        var user = UserLogic.Create("prof_test_user","passhash","Prof","Tester");
        var token = GenerateToken(user, RoleNames.Professor);

        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/student/pre-registration/eligible-courses?academicTermId=1");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetEligibleCourses_WithInvalidTermId_ReturnsBadRequest()
    {
        // Arrange
        var user = UserLogic.Create("student_invalid_term","passhash","Student","Tester");
        var token = GenerateToken(user, RoleNames.Student);

        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/student/pre-registration/eligible-courses?academicTermId=0");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetEligibleCourses_BusinessRulesScenario_VerifiesAllEligibilityRules()
    {
        // ── Arrange: Seed unique scenario in database ─────────────────────────
        var suffix = Guid.NewGuid().ToString("N")[..8];

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        // 1. Academic Term
        var term = AcademicTermLogic.Create($"T_{suffix}",$"Term {suffix}",DateTime.UtcNow,DateTime.UtcNow.AddMonths(4));
        context.AcademicTerms.Add(term);

        // 2. Academic Structure
        var faculty = FacultyLogic.Create($"F_{suffix}",$"Faculty {suffix}");
        context.Faculties.Add(faculty);
        await context.SaveChangesAsync();

        var department = DepartmentLogic.Create(faculty.Id,$"D_{suffix}",$"Department {suffix}");
        context.Departments.Add(department);
        await context.SaveChangesAsync();

        var major = MajorLogic.Create(department.Id,$"M_{suffix}",$"Major {suffix}");
        context.Majors.Add(major);

        // 3. User & Student
        var user = UserLogic.Create($"std_{suffix}","hash123","Ali","Rezaei");
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var student = StudentLogic.Create(user.Id,$"ST_{suffix}",major.Id,1403);
        context.Students.Add(student);

        // 4. Courses:
        // C1: No prerequisite
        // C2: Prerequisite = C1 (unmet)
        // C3: Passed course
        // C4: Prerequisite = C3 (met)
        var c1 = CourseLogic.Create($"C1_{suffix}","Math 1",3);
        var c2 = CourseLogic.Create($"C2_{suffix}","Math 2",3);
        var c3 = CourseLogic.Create($"C3_{suffix}","Physics 1",3);
        var c4 = CourseLogic.Create($"C4_{suffix}","Physics 2",3);

        context.Courses.AddRange(c1, c2, c3, c4);
        await context.SaveChangesAsync();

        // Add prerequisites: C2 requires C1, C4 requires C3
        CourseLogic.AddPrerequisite(
        // Add prerequisites: C2 requires C1, C4 requires C3
        c2,c1.Id);
        CourseLogic.AddPrerequisite(        c4,c3.Id);

        // 5. Active Curriculum
        var curriculum = CurriculumLogic.Create(major.Id,$"Curriculum {suffix}","1.0");
        context.Curriculums.Add(curriculum);
        await context.SaveChangesAsync();

        CurriculumLogic.AddCourse(
        curriculum,c1.Id,1,true);
        CurriculumLogic.AddCourse(        curriculum,c2.Id,2,true);
        CurriculumLogic.AddCourse(        curriculum,c3.Id,1,true);
        CurriculumLogic.AddCourse(        curriculum,c4.Id,2,true);

        // 6. Record Course History: C3 is PASSED by the student
        var c3History = StudentCourseHistoryLogic.Create(student.Id,c3.Id,term.Id);
        StudentCourseHistoryLogic.RecordGrade(        c3History,17.5m); // Status => Passed
        context.StudentCourseHistories.Add(c3History);

        await context.SaveChangesAsync();

        // Generate student JWT token for this student
        var token = GenerateToken(user, RoleNames.Student);

        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/v1/student/pre-registration/eligible-courses?academicTermId={term.Id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // ── Act ───────────────────────────────────────────────────────────────
        var response = await _client.SendAsync(request);

        // ── Assert ─────────────────────────────────────────────────────────────
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var courses = await response.Content.ReadFromJsonAsync<List<EligibleCourseDto>>();
        Assert.NotNull(courses);

        var returnedCourseIds = courses.Select(c => c.CourseId).ToHashSet();

        // 1. Course without prerequisite is returned: C1
        Assert.Contains(c1.Id, returnedCourseIds);

        // 2. Course with unmet prerequisite is NOT returned: C2 (requires C1 which is not passed)
        Assert.DoesNotContain(c2.Id, returnedCourseIds);

        // 3. Passed course is NOT returned: C3
        Assert.DoesNotContain(c3.Id, returnedCourseIds);

        // 4. Course with passed prerequisite is returned: C4 (requires C3 which was passed)
        Assert.Contains(c4.Id, returnedCourseIds);
    }
}
