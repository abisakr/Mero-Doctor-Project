using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mero_Doctor_Project.Migrations
{
    /// <inheritdoc />
    public partial class weeklyavailabilitychange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "DoctorWeeklyTimeRanges",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "DoctorWeeklyTimeRanges");
        }
    }
}
