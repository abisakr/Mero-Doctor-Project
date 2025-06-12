using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mero_Doctor_Project.Migrations
{
    /// <inheritdoc />
    public partial class appointmentUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Appointments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Appointments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionStatus",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "TransactionStatus",
                table: "Appointments");
        }
    }
}
