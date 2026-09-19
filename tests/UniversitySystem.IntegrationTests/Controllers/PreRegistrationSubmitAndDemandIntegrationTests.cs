using UniversitySystem.Api.Contracts;
using UniversitySystem.Persistence.Data;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UniversitySystem.Api.Controllers;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.AdminPreRegistration.DTOs;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Domain.Constants;
using UniversitySystem.Domain.Entities;
using Xunit;
usingUniversitySystem.Application.Common.Logic;

namespace UniversitySystem.IntegrationTests.Controllers;
public class PreRegistrationSubmitAndDemandIntegrationTests : IClassFixture<UniversityApiFactory>
{
    private readonly UniversityApiFactory _factory;
    private readonly HttpClient _client;
    private readonly ITokenService _tokenService;
    public PreRegistrationSubmitAndDemandIntegrationTests(UniversityApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        using var scope = factory.Services.CreateScope();
        _tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
    }

    private string GenerateToken(User user, string role)
    {
        var(token, _) = _tokenService.GenerateToken(user, new[] { role });
        return token;
    }

    private async Task<(User User, Student Student, AcademicTerm Term, Course Course1, Course Course2, Course Course3)> SeedScenarioAsync(string suffix)
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        // 1. Term
        var term = AcademicTermLogic.Create($"T_{suffix}",$"Term {suffix}",DateTime.UtcNow,DateTime.UtcNow.AddMonths(4));
        context.AcademicTerms.Add(term);
        // 2. Academic structure
        var faculty = FacultyLogic.Create($"F_{suffix}",$"Faculty {suffix}");
        context.Faculties.Add(faculty);
        await context.SaveChangesAsync();
        var department = DepartmentLogic.Create(faculty.Id,$"D_{suffix}",$"Department {suffix}");
        context.Departments.Add(department);
        await context.SaveChangesAsync();
        var major = MajorLogic.Create(department.Id,$"M_{suffix}",$"Major {suffix}");
        context.Majors.Add(major);
        // 3. User & Student
        var user = UserLogic.Create($"std_{suffix}","hash123","Ali","Rezai");
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var student = StudentLogic.Create(user.Id,$"ST_{suffix}",major.Id,1403);
        context.Students.Add(student);
        // 4. Courses (no prerequisites so all are eligible)
        var c1 = CourseLogic.Create($"C1_{suffix}","Mathematics",3);
        var c2 = CourseLogic.Create($"C2_{suffix}","Physics",2);
        var c3 = CourseLogic.Create($"C3_{suffix}","Programming",4);
        context.Courses.AddRange(c1, c2, c3);
        await context.SaveChangesAsync();
        // 5. Curriculum
        var curriculum = CurriculumLogic.Create(major.Id,$"Curriculum {suffix}","1.0");
        context.Curriculums.Add(curriculum);
        await context.SaveChangesAsync();
        CurriculumLogic.AddCourse(        curriculum,c1.Id,1,true);
        CurriculumLogic.AddCourse(        curriculum,c2.Id,1,true);
        CurriculumLogic.AddCourse(        curriculum,c3.Id,1,true);
        await context.SaveChangesAsync();
        return (user, student, term, c1, c2, c3);
    }

    private async Task<(User User, Student Student)> AddExtraStudentAsync(AcademicTerm term, Major major, string suffix)
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var user = UserLogic.Create($"std2_{suffix}","hash123","Sara","Ahmadi");
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var student = StudentLogic.Create(user.Id,$"ST2_{suffix}",major.Id,1403);
        context.Students.Add(student);
        await context.SaveChangesAsync();
        return (user, student);
    }

    [Fact]
    public async Task Draft_CanBeSubmitted_Successfully()
    {
        // Arrange
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var(user, _, term, c1, _, _) = await SeedScenarioAsync(suffix);
        var token = GenerateToken(user, RoleNames.Student);
        // 1. Create draft
        var saveBody = new SaveStudentPreRegistrationRequest
        {
            Courses = [new()
            {
                CourseId = c1.Id,
                Priority = 1
            }

            ]
        };
        using var putReq = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/student/pre-registration/{term.Id}");
        putReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        putReq.Content = JsonContent.Create(saveBody);
        var putRes = await _client.SendAsync(putReq);
        Assert.Equal(HttpStatusCode.OK, putRes.StatusCode);
        // 2. Submit draft
        using var submitReq = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/student/pre-registration/{term.Id}/submit");
        submitReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        // Act
        var submitRes = await _client.SendAsync(submitReq);
        // Assert
        Assert.Equal(HttpStatusCode.OK, submitRes.StatusCode);
        var result = await submitRes.Content.ReadFromJsonAsync<StudentPreRegistrationDto>();
        Assert.NotNull(result);
        Assert.Equal("Submitted", result.Status);
        Assert.NotNull(result.SubmittedAt);
        Assert.Single(result.Courses);
        Assert.Equal(c1.Id, result.Courses.First().CourseId);
        // Verify in DB
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var preReg = await context.StudentPreRegistrations.FirstOrDefaultAsync(pr => pr.Id == result.PreRegistrationId);
        Assert.NotNull(preReg);
        Assert.Equal(Domain.Enums.RequestStatus.Submitted, preReg.Status);
        Assert.NotNull(preReg.SubmittedAt);
    }

    [Fact]
    public async Task EmptyDraft_CannotBeSubmitted()
    {
        // Arrange
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var(user, student, term, _, _, _) = await SeedScenarioAsync(suffix);
        var token = GenerateToken(user, RoleNames.Student);
        // Create empty draft directly in DB
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var draft = StudentPreRegistrationLogic.Create(student.Id,term.Id);
            context.StudentPreRegistrations.Add(draft);
            await context.SaveChangesAsync();
        }

        // Act - attempt submit
        using var submitReq = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/student/pre-registration/{term.Id}/submit");
        submitReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.SendAsync(submitReq);
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AlreadySubmittedRequest_CannotBeSubmittedAgain()
    {
        // Arrange
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var(user, _, term, c1, _, _) = await SeedScenarioAsync(suffix);
        var token = GenerateToken(user, RoleNames.Student);
        // 1. Create draft
        var saveBody = new SaveStudentPreRegistrationRequest
        {
            Courses = [new()
            {
                CourseId = c1.Id,
                Priority = 1
            }

            ]
        };
        using var putReq = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/student/pre-registration/{term.Id}");
        putReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        putReq.Content = JsonContent.Create(saveBody);
        await _client.SendAsync(putReq);
        // 2. First submit -> OK
        using var submitReq1 = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/student/pre-registration/{term.Id}/submit");
        submitReq1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var res1 = await _client.SendAsync(submitReq1);
        Assert.Equal(HttpStatusCode.OK, res1.StatusCode);
        // 3. Second submit -> BadRequest
        using var submitReq2 = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/student/pre-registration/{term.Id}/submit");
        submitReq2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var res2 = await _client.SendAsync(submitReq2);
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, res2.StatusCode);
    }

    [Fact]
    public async Task SubmittedRequest_CannotBeEdited()
    {
        // Arrange
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var(user, _, term, c1, c2, _) = await SeedScenarioAsync(suffix);
        var token = GenerateToken(user, RoleNames.Student);
        // 1. Create draft
        var initialBody = new SaveStudentPreRegistrationRequest
        {
            Courses = [new()
            {
                CourseId = c1.Id,
                Priority = 1
            }

            ]
        };
        using var putReq1 = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/student/pre-registration/{term.Id}");
        putReq1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        putReq1.Content = JsonContent.Create(initialBody);
        await _client.SendAsync(putReq1);
        // 2. Submit
        using var submitReq = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/student/pre-registration/{term.Id}/submit");
        submitReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var submitRes = await _client.SendAsync(submitReq);
        Assert.Equal(HttpStatusCode.OK, submitRes.StatusCode);
        // 3. Try to edit after submit
        var editBody = new SaveStudentPreRegistrationRequest
        {
            Courses = [new()
            {
                CourseId = c2.Id,
                Priority = 1
            }

            ]
        };
        using var putReq2 = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/student/pre-registration/{term.Id}");
        putReq2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        putReq2.Content = JsonContent.Create(editBody);
        // Act
        var editRes = await _client.SendAsync(putReq2);
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, editRes.StatusCode);
    }

    [Fact]
    public async Task OnlySubmittedRequests_AffectDemand()
    {
        // Arrange
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var(user, _, term, c1, _, _) = await SeedScenarioAsync(suffix);
        var studentToken = GenerateToken(user, RoleNames.Student);
        var adminUser = UserLogic.Create($"adm_{suffix}","hash123","Admin","User");
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
        }

        var adminToken = GenerateToken(adminUser, RoleNames.EducationAdmin);
        // 1. Student creates a draft (NOT submitted)
        var saveBody = new SaveStudentPreRegistrationRequest
        {
            Courses = [new()
            {
                CourseId = c1.Id,
                Priority = 1
            }

            ]
        };
        using var putReq = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/student/pre-registration/{term.Id}");
        putReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", studentToken);
        putReq.Content = JsonContent.Create(saveBody);
        var putRes = await _client.SendAsync(putReq);
        Assert.Equal(HttpStatusCode.OK, putRes.StatusCode);
        // 2. Admin queries demand while in Draft
        using var demandReq1 = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/admin/pre-registration/demand?academicTermId={term.Id}");
        demandReq1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        var demandRes1 = await _client.SendAsync(demandReq1);
        Assert.Equal(HttpStatusCode.OK, demandRes1.StatusCode);
        var demandList1 = await demandRes1.Content.ReadFromJsonAsync<List<CourseDemandDto>>();
        Assert.NotNull(demandList1);
        Assert.DoesNotContain(demandList1, d => d.CourseId == c1.Id);
        // 3. Student submits the pre-registration
        using var submitReq = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/student/pre-registration/{term.Id}/submit");
        submitReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", studentToken);
        var submitRes = await _client.SendAsync(submitReq);
        Assert.Equal(HttpStatusCode.OK, submitRes.StatusCode);
        // 4. Admin queries demand after Submit
        using var demandReq2 = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/admin/pre-registration/demand?academicTermId={term.Id}");
        demandReq2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        var demandRes2 = await _client.SendAsync(demandReq2);
        Assert.Equal(HttpStatusCode.OK, demandRes2.StatusCode);
        var demandList2 = await demandRes2.Content.ReadFromJsonAsync<List<CourseDemandDto>>();
        Assert.NotNull(demandList2);
        var c1Demand = demandList2.FirstOrDefault(d => d.CourseId == c1.Id);
        Assert.NotNull(c1Demand);
        Assert.Equal(1, c1Demand.StudentCount);
    }

    [Fact]
    public async Task DemandCount_IsCalculatedCorrectly_AndSortedDescending()
    {
        // Arrange
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var(user1, student1, term, c1, c2, c3) = await SeedScenarioAsync(suffix);
        // Add 2 extra students in the same major
        Major major;
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            major = await context.Majors.FirstAsync(m => m.Id == student1.MajorId);
        }

        var(user2, student2) = await AddExtraStudentAsync(term, major, suffix + "b");
        var(user3, student3) = await AddExtraStudentAsync(term, major, suffix + "c");
        var token1 = GenerateToken(user1, RoleNames.Student);
        var token2 = GenerateToken(user2, RoleNames.Student);
        var token3 = GenerateToken(user3, RoleNames.Student);
        var adminUser = UserLogic.Create($"adm2_{suffix}","hash123","Admin","Super");
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
        }

        var adminToken = GenerateToken(adminUser, RoleNames.EducationAdmin);
        // Student 1 submits: Course 3 (p1), Course 1 (p2)
        var body1 = new SaveStudentPreRegistrationRequest
        {
            Courses = [new()
            {
                CourseId = c3.Id,
                Priority = 1
            }, new()
            {
                CourseId = c1.Id,
                Priority = 2
            }

            ]
        };
        await SaveAndSubmit(token1, term.Id, body1);
        // Student 2 submits: Course 3 (p2), Course 2 (p1)
        var body2 = new SaveStudentPreRegistrationRequest
        {
            Courses = [new()
            {
                CourseId = c3.Id,
                Priority = 2
            }, new()
            {
                CourseId = c2.Id,
                Priority = 1
            }

            ]
        };
        await SaveAndSubmit(token2, term.Id, body2);
        // Student 3 submits: Course 3 (p3)
        var body3 = new SaveStudentPreRegistrationRequest
        {
            Courses = [new()
            {
                CourseId = c3.Id,
                Priority = 3
            }

            ]
        };
        await SaveAndSubmit(token3, term.Id, body3);
        // Act - Query Demand
        using var demandReq = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/admin/pre-registration/demand?academicTermId={term.Id}");
        demandReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        var demandRes = await _client.SendAsync(demandReq);
        // Assert
        Assert.Equal(HttpStatusCode.OK, demandRes.StatusCode);
        var demandList = await demandRes.Content.ReadFromJsonAsync<List<CourseDemandDto>>();
        Assert.NotNull(demandList);
        Assert.Equal(3, demandList.Count);
        // First item should be Course 3 with highest demand (3 students)
        Assert.Equal(c3.Id, demandList[0].CourseId);
        Assert.Equal(3, demandList[0].StudentCount);
        // Average priority for C3: (1 + 2 + 3) / 3 = 2.0
        Assert.Equal(2.0, demandList[0].AveragePriority);
        // Total credits for C3: 3 students * 4 credits = 12
        Assert.Equal(12, demandList[0].TotalRequestedCredits);
        // Course 1 and Course 2 each have 1 student
        var c1Demand = demandList.First(d => d.CourseId == c1.Id);
        Assert.Equal(1, c1Demand.StudentCount);
        Assert.Equal(2.0, c1Demand.AveragePriority);
        Assert.Equal(3, c1Demand.TotalRequestedCredits);
        var c2Demand = demandList.First(d => d.CourseId == c2.Id);
        Assert.Equal(1, c2Demand.StudentCount);
        Assert.Equal(1.0, c2Demand.AveragePriority);
        Assert.Equal(2, c2Demand.TotalRequestedCredits);
    }

    [Fact]
    public async Task Student_CannotAccessAdminDemandEndpoint()
    {
        // Arrange
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var(user, _, term, _, _, _) = await SeedScenarioAsync(suffix);
        var studentToken = GenerateToken(user, RoleNames.Student);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/admin/pre-registration/demand?academicTermId={term.Id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", studentToken);
        // Act
        var response = await _client.SendAsync(request);
        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task EducationAdmin_CanAccessDemandEndpoint()
    {
        // Arrange
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var(_, _, term, _, _, _) = await SeedScenarioAsync(suffix);
        var adminUser = UserLogic.Create($"adm3_{suffix}","hash123","Admin","Boss");
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
        }

        var adminToken = GenerateToken(adminUser, RoleNames.EducationAdmin);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/admin/pre-registration/demand?academicTermId={term.Id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        // Act
        var response = await _client.SendAsync(request);
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<List<CourseDemandDto>>();
        Assert.NotNull(result);
    }

    private async Task SaveAndSubmit(string token, long termId, SaveStudentPreRegistrationRequest body)
    {
        using var putReq = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/student/pre-registration/{termId}");
        putReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        putReq.Content = JsonContent.Create(body);
        var putRes = await _client.SendAsync(putReq);
        Assert.Equal(HttpStatusCode.OK, putRes.StatusCode);
        using var submitReq = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/student/pre-registration/{termId}/submit");
        submitReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var submitRes = await _client.SendAsync(submitReq);
        Assert.Equal(HttpStatusCode.OK, submitRes.StatusCode);
    }
}
