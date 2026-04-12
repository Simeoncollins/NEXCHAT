using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NEXCHAT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111113"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Bio", "Country", "DateJoined", "Email", "FirstName", "LastLogin", "LastName", "PasswordHash", "Phone", "PhotoPath", "Roles", "SecurityAnswer", "SecurityQuestion", "Status", "UserName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Aspiring writer and tech innovator...", "United States", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "proxima@gmail.com", "Proxima", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cen", "AQAAAAIAAYagAAAAEBWuvAH8bLsGDStNP11zDw42A3H2kcA+T0dYM/sVp1D2nS+hIy/85ADCgN9ShVURVw==", "1234567890", "Uploads/User/bettle.jpg", "[\"User\"]", "Jessie", "What was the name of your first pet?", "Online", "proximacen10" },
                    { new Guid("11111111-1111-1111-1111-111111111112"), "Night sky enthusiast and telescope collector", "Canada", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "astro@example.com", "Orion", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Starborn", "AQAAAAIAAYagAAAAEBWuvAH8bLsGDStNP11zDw42A3H2kcA+T0dYM/sVp1D2nS+hIy/85ADCgN9ShVURVw==", "555-1234", "Uploads/User/orion.jpg", "[\"User\"]", "Jessie", "What was the name of your first pet?", "Offline", "astro_photographer" },
                    { new Guid("11111111-1111-1111-1111-111111111113"), "Building the future of communication technology", "United Kingdom", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pioneer@tech.io", "Ada", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Innovator", "AQAAAAIAAYagAAAAEBWuvAH8bLsGDStNP11zDw42A3H2kcA+T0dYM/sVp1D2nS+hIy/85ADCgN9ShVURVw==", "+1-555-9876", "Uploads/User/ada.jpg", "[\"User\"]", "Jessie", "What was the name of your first pet?", "Offline", "tech_pioneer" }
                });
        }
    }
}
