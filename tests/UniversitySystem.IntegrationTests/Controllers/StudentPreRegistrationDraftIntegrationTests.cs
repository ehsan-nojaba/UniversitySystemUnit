using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UniversitySystem.Api.Controllers;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Domain.Constants;
using UniversitySystem.Domain.Entities;
using Xunit;

namespace UniversitySystem.IntegrationTests.Controllers;

public class StudentPreRegistrationDraftIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly ITokenService _tokenService;

    public StudentPreRegistrationDraftIntegrationTests(WebApplicationFactory<Program> factory)
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

    private async Task<(User User, Student Student, AcademicTerm Term, Course Eligible1, Course Eligible2, Course Ineligible)>
        SeedScenarioAsync(string suffix)
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        // 1. Term
        var term = new AcademicTerm($"T_{suffix}", $"Term {suffix}", DateTime.UtcNow, DateTime.UtcNow.AddMonths(4));
        context.AcademicTerms.Add(term);

        // 2. Academic structure
        var faculty = new Faculty($"F_{suffix}", $"Faculty {suffix}");
        context.Faculties.Add(faculty);
        await context.SaveChangesAsync();

        var department = new Department(faculty.Id, $"D_{suffix}", $"Department {suffix}");
        context.Departments.Add(department);
        await context.SaveChangesAsync();

        var major = new Major(department.Id, $"M_{suffix}", $"Major {suffix}");
        context.Majors.Add(major);

        // 3. User & Student
        var user = new User($"std_{suffix}", "hash123", "Reza", "Mohammadi");
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var student = new Student(user.Id, $"ST_{suffix}", major.Id, 1403);
        context.Students.Add(student);

        // 4. Courses:
        // Eligible1: No prerequisite (Math 1)
        // Ineligible: Prerequisite is Math 1 (unmet)
        // PrereqPassed: (Physics 1) passed in history
        // Eligible2: Prerequisite is Physics 1 (Physics 2)
        var c1 = new Course($"C1_{suffix}", "Math 1", 3);
        var cIneligible = new Course($"CIN_{suffix}", "Math 2", 3);
        var cPassed = new Course($"CP_{suffix}", "Physics 1", 3);
        var c2 = new Course($"C2_{suffix}", "Physics 2", 3);

        context.Courses.AddRange(c1, cIneligible, cPassed, c2);
        await context.SaveChangesAsync();

        cIneligible.AddPrerequisite(c1.Id);
        c2.AddPrerequisite(cPassed.Id);

        // 5. Curriculum
        var curriculum = new Curriculum(major.Id, $"Curriculum {suffix}", "1.0");
        context.Curriculums.Add(curriculum);
        await context.SaveChangesAsync();

        curriculum.AddCourse(c1.Id, 1, true);
        curriculum.AddCourse(cIneligible.Id, 2, true);
        curriculum.AddCourse(cPassed.Id, 1, true);
        curriculum.AddCourse(c2.Id, 2, true);

        // 6. History: Physics 1 is passed
        var history = new StudentCourseHistory(student.Id, cPassed.Id, term.Id);
        history.RecordGrade(18m);
        context.StudentCourseHistories.Add(history);

        await context.SaveChangesAsync();

        return (user, student, term, c1, c2, cIneligible);
    }

    [Fact]
    public async Task CreateAndGetNewDraft_Succeeds()
    {
        // Arrange
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var (user, _, term, c1, _, _) = await SeedScenarioAsync(suffix);
        var token = GenerateToken(user, RoleNames.Student);

        var requestBody = new SaveStudentPreRegistrationRequest
        {
            Courses = [new() { CourseId = c1.Id, Priority = 1 }]
        };

        using var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/student/pre-registration/{term.Id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(requestBody);

        // Act - Save Draft
        var response = await _client.SendAsync(request);

        // Assert - Save response
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var draft = await response.Content.ReadFromJsonAsync<StudentPreRegistrationDto>();
        Assert.NotNull(draft);
        Assert.Equal(term.Id, draft.AcademicTermId);
        Assert.Equal("Draft", draft.Status);
        Assert.Single(draft.Courses);
        Assert.Equal(c1.Id, draft.Courses.First().CourseId);
        Assert.Equal(c1.Credits, draft.TotalCredits);

        // Act - Get Draft
        using var getRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/student/pre-registration/{term.Id}");
        getRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var getResponse = await _client.SendAsync(getRequest);

        // Assert - Get response
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var retrievedDraft = await getResponse.Content.ReadFromJsonAsync<StudentPreRegistrationDto>();
        Assert.NotNull(retrievedDraft);
        Assert.Equal(draft.PreRegistrationId, retrievedDraft.PreRegistrationId);
        Assert.Single(retrievedDraft.Courses);
    }

    [Fact]
    public async Task UpdateExistingDraft_UpdatesCoursesAndTotalCredits()
    {
        // Arrange
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var (user, _, term, c1, c2, _) = await SeedScenarioAsync(suffix);
        var token = GenerateToken(user, RoleNames.Student);

        // 1. First save: only C1
        var initialBody = new SaveStudentPreRegistrationRequest
        {
            Courses = [new() { CourseId = c1.Id, Priority = 1 }]
        };
        using var req1 = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/student/pre-registration/{term.Id}");
        req1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        req1.Content = JsonContent.Create(initialBody);
        var res1 = await _client.SendAsync(req1);
        Assert.Equal(HttpStatusCode.OK, res1.StatusCode);

        // 2. Update: replace C1 with C2, or have both C1 and C2
        var updatedBody = new SaveStudentPreRegistrationRequest
        {
            Courses = [new() { CourseId = c2.Id, Priority = 1 }, new() { CourseId = c1.Id, Priority = 2 }]
        };
        using var req2 = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/student/pre-registration/{term.Id}");
        req2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        req2.Content = JsonContent.Create(updatedBody);

        // Act
        var res2 = await _client.SendAsync(req2);

        // Assert
        Assert.Equal(HttpStatusCode.OK, res2.StatusCode);
        var updatedDraft = await res2.Content.ReadFromJsonAsync<StudentPreRegistrationDto>();
        Assert.NotNull(updatedDraft);
        Assert.Equal(2, updatedDraft.Courses.Count);
        Assert.Equal(c1.Credits + c2.Credits, updatedDraft.TotalCredits);
        Assert.Equal(c2.Id, updatedDraft.Courses.ElementAt(0).CourseId); // Priority 1
        Assert.Equal(c1.Id, updatedDraft.Courses.ElementAt(1).CourseId); // Priority 2
    }

    [Fact]
    public async Task SaveDraft_WithDuplicateCourses_ReturnsBadRequest()
    {
        // Arrange
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var (user, _, term, c1, _, _) = await SeedScenarioAsync(suffix);
        var token = GenerateToken(user, RoleNames.Student);

        var requestBody = new SaveStudentPreRegistrationRequest
        {
            Courses = [new() { CourseId = c1.Id, Priority = 1 }, new() { CourseId = c1.Id, Priority = 2 }]
        };

        using var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/student/pre-registration/{term.Id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(requestBody);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SaveDraft_WithIneligibleCourse_ReturnsBadRequest()
    {
        // Arrange
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var (user, _, term, _, _, cIneligible) = await SeedScenarioAsync(suffix);
        var token = GenerateToken(user, RoleNames.Student);

        var requestBody = new SaveStudentPreRegistrationRequest
        {
            Courses = [new() { CourseId = cIneligible.Id, Priority = 1 }]
        };

        using var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/student/pre-registration/{term.Id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(requestBody);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SaveDraft_WhenPreRegistrationIsAlreadySubmitted_ReturnsBadRequest()
    {
        // Arrange
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var (user, student, term, c1, _, _) = await SeedScenarioAsync(suffix);
        var token = GenerateToken(user, RoleNames.Student);

        // Create and submit pre-registration directly in DB
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var preReg = new StudentPreRegistration(student.Id, term.Id);
            preReg.AddCourse(c1.Id, 1);
            preReg.Submit(DateTime.UtcNow);
            context.StudentPreRegistrations.Add(preReg);
            await context.SaveChangesAsync();
        }

        // Try to update the submitted pre-registration
        var requestBody = new SaveStudentPreRegistrationRequest
        {
            Courses = [new() { CourseId = c1.Id, Priority = 1 }]
        };

        using var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/student/pre-registration/{term.Id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(requestBody);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SaveDraft_StudentCannotEditAnotherStudentsRequest()
    {
        // Arrange
        var suffixA = Guid.NewGuid().ToString("N")[..8];
        var (userA, studentA, term, c1A, _, _) = await SeedScenarioAsync(suffixA);

        // Create student B in the same term
        var suffixB = Guid.NewGuid().ToString("N")[..8];
        var (userB, studentB, _, c1B, _, _) = await SeedScenarioAsync(suffixB);

        var tokenA = GenerateToken(userA, RoleNames.Student);
        var tokenB = GenerateToken(userB, RoleNames.Student);

        // 1. Student A saves a draft with C1A
        var requestBodyA = new SaveStudentPreRegistrationRequest
        {
            Courses = [new() { CourseId = c1A.Id, Priority = 1 }]
        };
        using var reqA = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/student/pre-registration/{term.Id}");
        reqA.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenA);
        reqA.Content = JsonContent.Create(requestBodyA);
        var resA = await _client.SendAsync(reqA);
        Assert.Equal(HttpStatusCode.OK, resA.StatusCode);

        // 2. Student B sends PUT with C1B
        var requestBodyB = new SaveStudentPreRegistrationRequest
        {
            Courses = [new() { CourseId = c1B.Id, Priority = 1 }]
        };
        using var reqB = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/student/pre-registration/{term.Id}");
        reqB.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenB);
        reqB.Content = JsonContent.Create(requestBodyB);
        var resB = await _client.SendAsync(reqB);
        Assert.Equal(HttpStatusCode.OK, resB.StatusCode);

        // 3. Verify in DB that Student A's draft still has C1A, NOT C1B
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var draftA = await context.StudentPreRegistrations
            .Include(pr => pr.Items)
            .FirstOrDefaultAsync(pr => pr.StudentId == studentA.Id && pr.AcademicTermId == term.Id);

        Assert.NotNull(draftA);
        Assert.Single(draftA.Items);
        Assert.Equal(c1A.Id, draftA.Items.First().CourseId);
    }
}
