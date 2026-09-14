using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.AdminPlanning.DTOs;
using UniversitySystem.Domain.Constants;
using UniversitySystem.Domain.Entities;
using Xunit;

namespace UniversitySystem.IntegrationTests.Controllers;

public class AdminPlanningOverviewIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly IServiceProvider _services;
    private readonly ITokenService _tokenService;

    public AdminPlanningOverviewIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _services = factory.Services;
        using var scope = factory.Services.CreateScope();
        _tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
    }

    private string GenerateToken(User user, string role)
    {
        var (token, _) = _tokenService.GenerateToken(user, [role]);
        return token;
    }

    private async Task<(AcademicTerm Term, Course Course1, Course Course2, Course Course3, Major Major, User AdminUser)>
        SeedBaseScenarioAsync(string suffix)
    {
        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var term = new AcademicTerm($"T_{suffix}", $"Term {suffix}", DateTime.UtcNow, DateTime.UtcNow.AddMonths(4));
        context.AcademicTerms.Add(term);

        var faculty = new Faculty($"F_{suffix}", $"Faculty {suffix}");
        context.Faculties.Add(faculty);
        await context.SaveChangesAsync();

        var department = new Department(faculty.Id, $"D_{suffix}", $"Department {suffix}");
        context.Departments.Add(department);
        await context.SaveChangesAsync();

        var major = new Major(department.Id, $"M_{suffix}", $"Major {suffix}");
        context.Majors.Add(major);

        var c1 = new Course($"C1_{suffix}", "Algorithms", 3);
        var c2 = new Course($"C2_{suffix}", "Databases", 3);
        var c3 = new Course($"C3_{suffix}", "Networks", 3);
        context.Courses.AddRange(c1, c2, c3);

        var adminUser = new User($"adm_{suffix}", "hash", "Admin", "Chief");
        context.Users.Add(adminUser);
        await context.SaveChangesAsync();

        return (term, c1, c2, c3, major, adminUser);
    }

    private async Task<Student> AddStudentAsync(Major major, string suffix)
    {
        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var user = new User($"std_{suffix}", "hash", $"First_{suffix}", $"Last_{suffix}");
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var student = new Student(user.Id, $"ST_{suffix}", major.Id, 1403);
        context.Students.Add(student);
        await context.SaveChangesAsync();
        return student;
    }

    private async Task<(Professor Professor, User User)> AddProfessorAsync(string firstName, string lastName, string suffix)
    {
        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var user = new User($"prof_{suffix}", "hash", firstName, lastName);
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var professor = new Professor(user.Id, $"P_{suffix}");
        context.Professors.Add(professor);
        await context.SaveChangesAsync();
        return (professor, user);
    }

    [Fact]
    public async Task Submitted_Student_Requests_Affect_Demand()
    {
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var (term, c1, c2, _, major, adminUser) = await SeedBaseScenarioAsync(suffix);
        var student1 = await AddStudentAsync(major, suffix + "1");
        var student2 = await AddStudentAsync(major, suffix + "2");

        using (var scope = _services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var reg1 = new StudentPreRegistration(student1.Id, term.Id);
            reg1.AddCourse(c1.Id, priority: 1);
            reg1.Submit(DateTime.UtcNow);

            var reg2 = new StudentPreRegistration(student2.Id, term.Id);
            reg2.AddCourse(c1.Id, priority: 2);
            reg2.AddCourse(c2.Id, priority: 1);
            reg2.Submit(DateTime.UtcNow);

            context.StudentPreRegistrations.AddRange(reg1, reg2);
            await context.SaveChangesAsync();
        }

        var adminToken = GenerateToken(adminUser, RoleNames.EducationAdmin);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/admin/planning/overview?academicTermId={term.Id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var overview = await response.Content.ReadFromJsonAsync<AcademicPlanningOverviewDto>();
        Assert.NotNull(overview);
        Assert.Equal(term.Id, overview.AcademicTermId);

        var c1Overview = overview.Courses.FirstOrDefault(c => c.CourseId == c1.Id);
        var c2Overview = overview.Courses.FirstOrDefault(c => c.CourseId == c2.Id);
        Assert.NotNull(c1Overview);
        Assert.NotNull(c2Overview);
        Assert.Equal(2, c1Overview.StudentDemandCount);
        Assert.Equal(1, c2Overview.StudentDemandCount);
    }

    [Fact]
    public async Task Draft_Student_Requests_Are_Ignored()
    {
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var (term, c1, c2, _, major, adminUser) = await SeedBaseScenarioAsync(suffix);
        var student1 = await AddStudentAsync(major, suffix + "1");
        var student2 = await AddStudentAsync(major, suffix + "2");

        using (var scope = _services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var submittedReg = new StudentPreRegistration(student1.Id, term.Id);
            submittedReg.AddCourse(c1.Id, priority: 1);
            submittedReg.Submit(DateTime.UtcNow);

            var draftReg = new StudentPreRegistration(student2.Id, term.Id);
            draftReg.AddCourse(c2.Id, priority: 1);

            context.StudentPreRegistrations.AddRange(submittedReg, draftReg);
            await context.SaveChangesAsync();
        }

        var adminToken = GenerateToken(adminUser, RoleNames.EducationAdmin);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/admin/planning/overview?academicTermId={term.Id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var overview = await response.Content.ReadFromJsonAsync<AcademicPlanningOverviewDto>();
        Assert.NotNull(overview);
        Assert.Contains(overview.Courses, c => c.CourseId == c1.Id && c.StudentDemandCount == 1);
        Assert.DoesNotContain(overview.Courses, c => c.CourseId == c2.Id);
    }

    [Fact]
    public async Task Submitted_Professor_Requests_Are_Included()
    {
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var (term, c1, _, _, _, adminUser) = await SeedBaseScenarioAsync(suffix);
        var (prof1, _) = await AddProfessorAsync("Maryam", "Mirzakhani", suffix + "p1");
        var (prof2, _) = await AddProfessorAsync("Ali", "Mohammadi", suffix + "p2");

        using (var scope = _services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var req1 = new ProfessorTeachingRequest(prof1.Id, term.Id);
            req1.AddCourse(c1.Id, priority: 1);
            req1.Submit(DateTime.UtcNow);

            var req2 = new ProfessorTeachingRequest(prof2.Id, term.Id);
            req2.AddCourse(c1.Id, priority: 2);
            req2.Submit(DateTime.UtcNow);

            context.ProfessorTeachingRequests.AddRange(req1, req2);
            await context.SaveChangesAsync();
        }

        var adminToken = GenerateToken(adminUser, RoleNames.EducationAdmin);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/admin/planning/overview?academicTermId={term.Id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var overview = await response.Content.ReadFromJsonAsync<AcademicPlanningOverviewDto>();
        Assert.NotNull(overview);
        var c1Overview = overview.Courses.FirstOrDefault(c => c.CourseId == c1.Id);
        Assert.NotNull(c1Overview);
        Assert.Equal(0, c1Overview.StudentDemandCount);
        Assert.Equal(2, c1Overview.InterestedProfessorsCount);
        Assert.Equal(2, c1Overview.InterestedProfessors.Count);

        var firstProf = c1Overview.InterestedProfessors.ElementAt(0);
        Assert.Equal(prof1.Id, firstProf.ProfessorId);
        Assert.Equal("Maryam Mirzakhani", firstProf.ProfessorFullName);
        Assert.Equal(1, firstProf.Priority);

        var secondProf = c1Overview.InterestedProfessors.ElementAt(1);
        Assert.Equal(prof2.Id, secondProf.ProfessorId);
        Assert.Equal("Ali Mohammadi", secondProf.ProfessorFullName);
        Assert.Equal(2, secondProf.Priority);
    }

    [Fact]
    public async Task Draft_Professor_Requests_Are_Ignored()
    {
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var (term, c1, c2, _, _, adminUser) = await SeedBaseScenarioAsync(suffix);
        var (prof1, _) = await AddProfessorAsync("Hassan", "Ghasemi", suffix + "p1");
        var (prof2, _) = await AddProfessorAsync("Zahra", "Hosseini", suffix + "p2");

        using (var scope = _services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var submittedReq = new ProfessorTeachingRequest(prof1.Id, term.Id);
            submittedReq.AddCourse(c1.Id, priority: 1);
            submittedReq.Submit(DateTime.UtcNow);

            var draftReq = new ProfessorTeachingRequest(prof2.Id, term.Id);
            draftReq.AddCourse(c2.Id, priority: 1);

            context.ProfessorTeachingRequests.AddRange(submittedReq, draftReq);
            await context.SaveChangesAsync();
        }

        var adminToken = GenerateToken(adminUser, RoleNames.EducationAdmin);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/admin/planning/overview?academicTermId={term.Id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var overview = await response.Content.ReadFromJsonAsync<AcademicPlanningOverviewDto>();
        Assert.NotNull(overview);
        Assert.Contains(overview.Courses, c => c.CourseId == c1.Id && c.InterestedProfessorsCount == 1);
        Assert.DoesNotContain(overview.Courses, c => c.CourseId == c2.Id);
    }

    [Fact]
    public async Task EducationAdmin_Can_Access()
    {
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var (term, _, _, _, _, adminUser) = await SeedBaseScenarioAsync(suffix);

        var adminToken = GenerateToken(adminUser, RoleNames.EducationAdmin);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/admin/planning/overview?academicTermId={term.Id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var overview = await response.Content.ReadFromJsonAsync<AcademicPlanningOverviewDto>();
        Assert.NotNull(overview);
        Assert.Equal(term.Id, overview.AcademicTermId);
    }

    [Theory]
    [InlineData(RoleNames.Student)]
    [InlineData(RoleNames.Professor)]
    public async Task Other_Roles_Cannot_Access(string forbiddenRole)
    {
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var (term, _, _, _, _, _) = await SeedBaseScenarioAsync(suffix);

        var user = new User($"user_{forbiddenRole}_{suffix}", "hash", "Other", "Role");
        using (var scope = _services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        var token = GenerateToken(user, forbiddenRole);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/admin/planning/overview?academicTermId={term.Id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Unauthenticated_User_Cannot_Access()
    {
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var (term, _, _, _, _, _) = await SeedBaseScenarioAsync(suffix);

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/admin/planning/overview?academicTermId={term.Id}");
        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Results_Are_Sorted_By_StudentDemandCount_Descending()
    {
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var (term, c1, c2, c3, major, adminUser) = await SeedBaseScenarioAsync(suffix);
        var s1 = await AddStudentAsync(major, suffix + "s1");
        var s2 = await AddStudentAsync(major, suffix + "s2");
        var s3 = await AddStudentAsync(major, suffix + "s3");

        using (var scope = _services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            // C2 gets 3 demands
            var reg1 = new StudentPreRegistration(s1.Id, term.Id);
            reg1.AddCourse(c2.Id, 1);
            reg1.AddCourse(c1.Id, 2);
            reg1.Submit(DateTime.UtcNow);

            var reg2 = new StudentPreRegistration(s2.Id, term.Id);
            reg2.AddCourse(c2.Id, 1);
            reg2.Submit(DateTime.UtcNow);

            var reg3 = new StudentPreRegistration(s3.Id, term.Id);
            reg3.AddCourse(c2.Id, 1);
            reg3.AddCourse(c3.Id, 2);
            reg3.Submit(DateTime.UtcNow);

            context.StudentPreRegistrations.AddRange(reg1, reg2, reg3);
            await context.SaveChangesAsync();
        }

        var adminToken = GenerateToken(adminUser, RoleNames.EducationAdmin);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/admin/planning/overview?academicTermId={term.Id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var overview = await response.Content.ReadFromJsonAsync<AcademicPlanningOverviewDto>();
        Assert.NotNull(overview);
        var list = overview.Courses.ToList();
        Assert.Equal(3, list.Count);
        Assert.Equal(c2.Id, list[0].CourseId);
        Assert.Equal(3, list[0].StudentDemandCount);
        Assert.Equal(1, list[1].StudentDemandCount);
        Assert.Equal(1, list[2].StudentDemandCount);
    }
}
