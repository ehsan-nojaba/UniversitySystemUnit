using UniversitySystem.Persistence.Data;
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
using UniversitySystem.Application.Common.Logic;

namespace UniversitySystem.IntegrationTests.Controllers;

public class AdminPlanningOverviewIntegrationTests : IClassFixture<UniversityApiFactory>
{
    private readonly HttpClient _client;
    private readonly IServiceProvider _services;
    private readonly ITokenService _tokenService;

    public AdminPlanningOverviewIntegrationTests(UniversityApiFactory factory)
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

        var term = AcademicTermLogic.Create($"T_{suffix}",$"Term {suffix}",DateTime.UtcNow,DateTime.UtcNow.AddMonths(4));
        context.AcademicTerms.Add(term);

        var faculty = FacultyLogic.Create($"F_{suffix}",$"Faculty {suffix}");
        context.Faculties.Add(faculty);
        await context.SaveChangesAsync();

        var department = DepartmentLogic.Create(faculty.Id,$"D_{suffix}",$"Department {suffix}");
        context.Departments.Add(department);
        await context.SaveChangesAsync();

        var major = MajorLogic.Create(department.Id,$"M_{suffix}",$"Major {suffix}");
        context.Majors.Add(major);

        var c1 = CourseLogic.Create($"C1_{suffix}","Algorithms",3);
        var c2 = CourseLogic.Create($"C2_{suffix}","Databases",3);
        var c3 = CourseLogic.Create($"C3_{suffix}","Networks",3);
        context.Courses.AddRange(c1, c2, c3);

        var adminUser = UserLogic.Create($"adm_{suffix}","hash","Admin","Chief");
        context.Users.Add(adminUser);
        await context.SaveChangesAsync();

        return (term, c1, c2, c3, major, adminUser);
    }

    private async Task<Student> AddStudentAsync(Major major, string suffix)
    {
        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var user = UserLogic.Create($"std_{suffix}","hash",$"First_{suffix}",$"Last_{suffix}");
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var student = StudentLogic.Create(user.Id,$"ST_{suffix}",major.Id,1403);
        context.Students.Add(student);
        await context.SaveChangesAsync();
        return student;
    }

    private async Task<(Professor Professor, User User)> AddProfessorAsync(string firstName, string lastName, string suffix)
    {
        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var user = UserLogic.Create($"prof_{suffix}","hash",firstName,lastName);
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var professor = ProfessorLogic.Create(user.Id,$"P_{suffix}");
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
            var reg1 = StudentPreRegistrationLogic.Create(student1.Id,term.Id);
            StudentPreRegistrationLogic.AddCourse(            reg1,c1.Id,priority: 1);
            StudentPreRegistrationLogic.Submit(            reg1,DateTime.UtcNow);

            var reg2 = StudentPreRegistrationLogic.Create(student2.Id,term.Id);
            StudentPreRegistrationLogic.AddCourse(            reg2,c1.Id,priority: 2);
            StudentPreRegistrationLogic.AddCourse(            reg2,c2.Id,priority: 1);
            StudentPreRegistrationLogic.Submit(            reg2,DateTime.UtcNow);

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
            var submittedReg = StudentPreRegistrationLogic.Create(student1.Id,term.Id);
            StudentPreRegistrationLogic.AddCourse(            submittedReg,c1.Id,priority: 1);
            StudentPreRegistrationLogic.Submit(            submittedReg,DateTime.UtcNow);

            var draftReg = StudentPreRegistrationLogic.Create(student2.Id,term.Id);
            StudentPreRegistrationLogic.AddCourse(            draftReg,c2.Id,priority: 1);

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
            var req1 = ProfessorTeachingRequestLogic.Create(prof1.Id,term.Id);
            ProfessorTeachingRequestLogic.AddCourse(            req1,c1.Id,priority: 1);
            ProfessorTeachingRequestLogic.Submit(            req1,DateTime.UtcNow);

            var req2 = ProfessorTeachingRequestLogic.Create(prof2.Id,term.Id);
            ProfessorTeachingRequestLogic.AddCourse(            req2,c1.Id,priority: 2);
            ProfessorTeachingRequestLogic.Submit(            req2,DateTime.UtcNow);

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
            var submittedReq = ProfessorTeachingRequestLogic.Create(prof1.Id,term.Id);
            ProfessorTeachingRequestLogic.AddCourse(            submittedReq,c1.Id,priority: 1);
            ProfessorTeachingRequestLogic.Submit(            submittedReq,DateTime.UtcNow);

            var draftReq = ProfessorTeachingRequestLogic.Create(prof2.Id,term.Id);
            ProfessorTeachingRequestLogic.AddCourse(            draftReq,c2.Id,priority: 1);

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

        var user = UserLogic.Create($"user_{forbiddenRole}_{suffix}","hash","Other","Role");
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
            var reg1 = StudentPreRegistrationLogic.Create(s1.Id,term.Id);
            StudentPreRegistrationLogic.AddCourse(            reg1,c2.Id,1);
            StudentPreRegistrationLogic.AddCourse(            reg1,c1.Id,2);
            StudentPreRegistrationLogic.Submit(            reg1,DateTime.UtcNow);

            var reg2 = StudentPreRegistrationLogic.Create(s2.Id,term.Id);
            StudentPreRegistrationLogic.AddCourse(            reg2,c2.Id,1);
            StudentPreRegistrationLogic.Submit(            reg2,DateTime.UtcNow);

            var reg3 = StudentPreRegistrationLogic.Create(s3.Id,term.Id);
            StudentPreRegistrationLogic.AddCourse(            reg3,c2.Id,1);
            StudentPreRegistrationLogic.AddCourse(            reg3,c3.Id,2);
            StudentPreRegistrationLogic.Submit(            reg3,DateTime.UtcNow);

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

