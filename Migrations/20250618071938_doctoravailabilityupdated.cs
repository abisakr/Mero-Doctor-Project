using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mero_Doctor_Project.Migrations
{
    /// <inheritdoc />
    public partial class doctoravailabilityupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "DoctorWeeklyTimeRanges");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "DoctorWeeklyAvailabilities");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "DoctorWeeklyTimeRanges",
                newName: "AvailableTime");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Appointments",
                newName: "AvailableTime");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Appointments",
                newName: "BookingDateTime");

            migrationBuilder.AddColumn<DateOnly>(
                name: "AvailableDate",
                table: "DoctorWeeklyAvailabilities",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "AvailableDate",
                table: "Appointments",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<bool>(
                name: "Visited",
                table: "Appointments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableDate",
                table: "DoctorWeeklyAvailabilities");

            migrationBuilder.DropColumn(
                name: "AvailableDate",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Visited",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "AvailableTime",
                table: "DoctorWeeklyTimeRanges",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "BookingDateTime",
                table: "Appointments",
                newName: "DateTime");

            migrationBuilder.RenameColumn(
                name: "AvailableTime",
                table: "Appointments",
                newName: "StartTime");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "DoctorWeeklyTimeRanges",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "DoctorWeeklyAvailabilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "Appointments",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }
    }
}
