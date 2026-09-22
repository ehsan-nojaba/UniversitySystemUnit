using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversitySystem.Persistence.Migrations;

/// <summary>
/// سقف دو نوبت پیش‌انتخاب را برای هر دانشجو و ترم ذخیره می‌کند.
/// </summary>
public partial class AddStudentPreRegistrationAttemptLimit : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "AttemptCount",
            table: "StudentPreRegistrations",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.Sql("UPDATE StudentPreRegistrations SET AttemptCount = 1 WHERE Status = 2 AND AttemptCount = 0");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AttemptCount",
            table: "StudentPreRegistrations");
    }
}
