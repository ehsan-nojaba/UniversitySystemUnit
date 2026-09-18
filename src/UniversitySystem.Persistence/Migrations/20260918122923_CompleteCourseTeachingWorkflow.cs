using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversitySystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CompleteCourseTeachingWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CourseId",
                table: "ProfessorAvailabilities",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinalized",
                table: "CourseOfferings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ProfessorCourses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfessorId = table.Column<long>(type: "bigint", nullable: false),
                    CourseId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessorCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfessorCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfessorCourses_Professors_ProfessorId",
                        column: x => x.ProfessorId,
                        principalTable: "Professors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorAvailabilities_CourseId",
                table: "ProfessorAvailabilities",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorCourses_CourseId",
                table: "ProfessorCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorCourses_ProfessorId_CourseId",
                table: "ProfessorCourses",
                columns: new[] { "ProfessorId", "CourseId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessorAvailabilities_Courses_CourseId",
                table: "ProfessorAvailabilities",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            // ارتباط‌های قبلی استاد و درس حفظ می‌شوند؛ زمان عمومی قدیمی برای درس‌های همان درخواست منتقل می‌شود.
            migrationBuilder.Sql("""
                INSERT INTO ProfessorCourses (ProfessorId, CourseId, CreatedAt)
                SELECT x.ProfessorId, x.CourseId, SYSUTCDATETIME()
                FROM (
                    SELECT r.ProfessorId, c.CourseId FROM ProfessorTeachingRequests r
                    INNER JOIN ProfessorTeachingRequestCourses c ON c.ProfessorTeachingRequestId = r.Id
                    UNION
                    SELECT a.ProfessorId, o.CourseId FROM TeachingAssignments a
                    INNER JOIN CourseOfferings o ON o.Id = a.CourseOfferingId
                ) x;

                INSERT INTO ProfessorAvailabilities (ProfessorTeachingRequestId, CourseId, DayOfWeek, StartTime, EndTime, CreatedAt, CreatedBy, LastModifiedAt, LastModifiedBy)
                SELECT a.ProfessorTeachingRequestId, c.CourseId, a.DayOfWeek, a.StartTime, a.EndTime, a.CreatedAt, a.CreatedBy, a.LastModifiedAt, a.LastModifiedBy
                FROM ProfessorAvailabilities a INNER JOIN ProfessorTeachingRequestCourses c ON c.ProfessorTeachingRequestId = a.ProfessorTeachingRequestId
                WHERE a.CourseId IS NULL;

                -- رکوردهای عمومی قدیمی برای حفظ تاریخچه باقی می‌مانند؛ خواندن فرایند جدید از نسخه مرتبط با درس است.
                -- کلاسی که قبلاً ثبت‌نام واقعی دارد در دسترس دانشجو باقی می‌ماند.
                UPDATE o SET IsFinalized = 1 FROM CourseOfferings o
                WHERE EXISTS (SELECT 1 FROM Enrollments e WHERE e.CourseOfferingId = o.Id AND e.Status = 1);
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfessorAvailabilities_Courses_CourseId",
                table: "ProfessorAvailabilities");

            migrationBuilder.DropTable(
                name: "ProfessorCourses");

            migrationBuilder.DropIndex(
                name: "IX_ProfessorAvailabilities_CourseId",
                table: "ProfessorAvailabilities");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "ProfessorAvailabilities");

            migrationBuilder.DropColumn(
                name: "IsFinalized",
                table: "CourseOfferings");
        }
    }
}
