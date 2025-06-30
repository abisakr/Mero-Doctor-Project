using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mero_Doctor_Project.Migrations
{
    /// <inheritdoc />
    public partial class xrayhistorymodelchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Confidence",
                table: "XRayRecords",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confidence",
                table: "XRayRecords");
        }
    }
}
