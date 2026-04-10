using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NEXCHAT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAngryCoolReactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reactions",
                keyColumn: "ReactionId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111119"));

            migrationBuilder.DeleteData(
                table: "Reactions",
                keyColumn: "ReactionId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111133"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Reactions",
                columns: new[] { "ReactionId", "EmojiPath", "ReactionName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111119"), "https://lottie.host/a4cff48f-156f-4114-b27e-cff7bd0f29a6/hE5NQSPEyA.lottie", "Angry" },
                    { new Guid("11111111-1111-1111-1111-111111111133"), "https://lottie.host/632a0f85-69e8-4b70-8d67-02c1c2e08365/XJNJAOQu8D.lottie", "Cool" }
                });
        }
    }
}
