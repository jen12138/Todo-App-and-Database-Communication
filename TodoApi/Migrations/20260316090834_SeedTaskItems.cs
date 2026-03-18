using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TodoApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedTaskItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TaskItems",
                columns: new[] { "Id", "CreatedAt", "Description", "IsCompleted", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Daily sync at 10:30 AM", false, "Team Standup" },
                    { 2, new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pick up veggies", true, "Grocery Run" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaskItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TaskItems",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
