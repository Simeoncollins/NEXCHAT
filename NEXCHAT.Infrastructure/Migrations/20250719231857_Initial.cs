using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NEXCHAT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    ReactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReactionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmojiPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.ReactionId);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    RefreshTokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.RefreshTokenId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecurityQuestion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecurityAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DateJoined = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Roles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateStartedUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsGroupConversation = table.Column<bool>(type: "bit", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupCoverPhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.ConversationId);
                    table.ForeignKey(
                        name: "FK_Conversations_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateSent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsSeen = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFriends",
                columns: table => new
                {
                    RequesterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFriends", x => new { x.RequesterId, x.ReceiverId });
                    table.ForeignKey(
                        name: "FK_UserFriends_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFriends_Users_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConversationParticipants",
                columns: table => new
                {
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationParticipants", x => new { x.ConversationId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ConversationParticipants_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "ConversationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConversationParticipants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConversationTypingUsers",
                columns: table => new
                {
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationTypingUsers", x => new { x.ConversationId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ConversationTypingUsers_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "ConversationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConversationTypingUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDelivered = table.Column<bool>(type: "bit", nullable: false),
                    DateSentUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "ConversationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessageReactions",
                columns: table => new
                {
                    UserReactedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageReactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageReactions", x => new { x.MessageId, x.UserReactedId });
                    table.ForeignKey(
                        name: "FK_MessageReactions_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "MessageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageReactions_Reactions_ReactionId",
                        column: x => x.ReactionId,
                        principalTable: "Reactions",
                        principalColumn: "ReactionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageReactions_Users_UserReactedId",
                        column: x => x.UserReactedId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessagesSeen",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeenAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagesSeen", x => new { x.MessageId, x.UserId });
                    table.ForeignKey(
                        name: "FK_MessagesSeen_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "MessageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessagesSeen_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Reactions",
                columns: new[] { "ReactionId", "EmojiPath", "ReactionName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111100"), "https://lottie.host/76e83562-9204-4362-9fcc-60a17e219d40/nrBKNNLnfZ.lottie", "Unhappy" },
                    { new Guid("11111111-1111-1111-1111-111111111114"), "https://lottie.host/91d7a732-3fef-4397-a427-dfc9ce7513fd/vhgRnJQiUz.lottie", "Laughing" },
                    { new Guid("11111111-1111-1111-1111-111111111115"), "https://lottie.host/861562a2-2975-4b7a-847a-f50431be25f0/D6hWtQp9EM.lottie", "Sad" },
                    { new Guid("11111111-1111-1111-1111-111111111116"), "https://lottie.host/9ef73a79-293c-496c-82f7-ed27c40d63eb/RGg5Ewhxx3.lottie", "Angel" },
                    { new Guid("11111111-1111-1111-1111-111111111117"), "https://lottie.host/30a2c4c6-0277-4b7d-8d85-f8fd6ddbdb88/Zg2ds75R2E.lottie", "Kiss" },
                    { new Guid("11111111-1111-1111-1111-111111111118"), "https://lottie.host/5bda14b0-e838-4074-bb58-e85f6608db6d/ovIILqAs4u.lottie", "Dissapointed" },
                    { new Guid("11111111-1111-1111-1111-111111111119"), "https://lottie.host/a4cff48f-156f-4114-b27e-cff7bd0f29a6/hE5NQSPEyA.lottie", "Angry" },
                    { new Guid("11111111-1111-1111-1111-111111111122"), "https://lottie.host/29d5ff89-287f-42d1-89bc-25b695e2ed7f/hZFip2GqwA.lottie", "Suprised" },
                    { new Guid("11111111-1111-1111-1111-111111111133"), "https://lottie.host/632a0f85-69e8-4b70-8d67-02c1c2e08365/XJNJAOQu8D.lottie", "Cool" },
                    { new Guid("11111111-1111-1111-1111-111111111144"), "https://lottie.host/1ba46ee9-b4d2-44c7-baa8-8fa0adc4ccb7/oc0HMXijhA.lottie", "Crying" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "11111111-1111-1111-1111-111111111155", null, "User", "USER" },
                    { "11111111-1111-1111-1111-111111111166", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Bio", "Country", "DateJoined", "Email", "FirstName", "LastLogin", "LastName", "PasswordHash", "Phone", "PhotoPath", "Roles", "SecurityAnswer", "SecurityQuestion", "Status", "UserName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Aspiring writer and tech innovator...", "United States", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "proxima@gmail.com", "Proxima", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cen", "AQAAAAIAAYagAAAAEBWuvAH8bLsGDStNP11zDw42A3H2kcA+T0dYM/sVp1D2nS+hIy/85ADCgN9ShVURVw==", "1234567890", "Uploads/User/bettle.jpg", "[\"User\"]", "Jessie", "What was the name of your first pet?", "Online", "proximacen10" },
                    { new Guid("11111111-1111-1111-1111-111111111112"), "Night sky enthusiast and telescope collector", "Canada", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "astro@example.com", "Orion", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Starborn", "AQAAAAIAAYagAAAAEBWuvAH8bLsGDStNP11zDw42A3H2kcA+T0dYM/sVp1D2nS+hIy/85ADCgN9ShVURVw==", "555-1234", "Uploads/User/orion.jpg", "[\"User\"]", "Jessie", "What was the name of your first pet?", "Offline", "astro_photographer" },
                    { new Guid("11111111-1111-1111-1111-111111111113"), "Building the future of communication technology", "United Kingdom", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pioneer@tech.io", "Ada", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Innovator", "AQAAAAIAAYagAAAAEBWuvAH8bLsGDStNP11zDw42A3H2kcA+T0dYM/sVp1D2nS+hIy/85ADCgN9ShVURVw==", "+1-555-9876", "Uploads/User/ada.jpg", "[\"User\"]", "Jessie", "What was the name of your first pet?", "Offline", "tech_pioneer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversationParticipants_ConversationId",
                table: "ConversationParticipants",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationParticipants_UserId",
                table: "ConversationParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_CreatorId",
                table: "Conversations",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationTypingUsers_ConversationId",
                table: "ConversationTypingUsers",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationTypingUsers_UserId",
                table: "ConversationTypingUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReactions_MessageId",
                table: "MessageReactions",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReactions_ReactionId",
                table: "MessageReactions",
                column: "ReactionId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReactions_UserReactedId",
                table: "MessageReactions",
                column: "UserReactedId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId",
                table: "Messages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesSeen_MessageId",
                table: "MessagesSeen",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesSeen_UserId",
                table: "MessagesSeen",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_ReactionName",
                table: "Reactions",
                column: "ReactionName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_NormalizedName",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserFriends_ReceiverId",
                table: "UserFriends",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFriends_RequesterId",
                table: "UserFriends",
                column: "RequesterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversationParticipants");

            migrationBuilder.DropTable(
                name: "ConversationTypingUsers");

            migrationBuilder.DropTable(
                name: "MessageReactions");

            migrationBuilder.DropTable(
                name: "MessagesSeen");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserFriends");

            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
