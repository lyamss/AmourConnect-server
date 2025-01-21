using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BackendClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id_User = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userIdGoogle = table.Column<string>(type: "text", nullable: false),
                    Pseudo = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EmailGoogle = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Profile_picture = table.Column<byte[]>(type: "bytea", nullable: true),
                    date_token_session_expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    token_session_user = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    sex = table.Column<string>(type: "text", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    account_created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id_User);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id_Message = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUserIssuer = table.Column<int>(type: "integer", nullable: false),
                    Id_UserReceiver = table.Column<int>(type: "integer", nullable: false),
                    message_content = table.Column<string>(type: "text", nullable: false),
                    Date_of_request = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id_Message);
                    table.ForeignKey(
                        name: "FK_Message_User_IdUserIssuer",
                        column: x => x.IdUserIssuer,
                        principalTable: "User",
                        principalColumn: "Id_User",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_User_Id_UserReceiver",
                        column: x => x.Id_UserReceiver,
                        principalTable: "User",
                        principalColumn: "Id_User",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestFriends",
                columns: table => new
                {
                    Id_RequestFriends = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUserIssuer = table.Column<int>(type: "integer", nullable: false),
                    Id_UserReceiver = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Date_of_request = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestFriends", x => x.Id_RequestFriends);
                    table.ForeignKey(
                        name: "FK_RequestFriends_User_IdUserIssuer",
                        column: x => x.IdUserIssuer,
                        principalTable: "User",
                        principalColumn: "Id_User",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestFriends_User_Id_UserReceiver",
                        column: x => x.Id_UserReceiver,
                        principalTable: "User",
                        principalColumn: "Id_User",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_Id_UserReceiver",
                table: "Message",
                column: "Id_UserReceiver");

            migrationBuilder.CreateIndex(
                name: "IX_Message_IdUserIssuer",
                table: "Message",
                column: "IdUserIssuer");

            migrationBuilder.CreateIndex(
                name: "IX_RequestFriends_Id_UserReceiver",
                table: "RequestFriends",
                column: "Id_UserReceiver");

            migrationBuilder.CreateIndex(
                name: "IX_RequestFriends_IdUserIssuer",
                table: "RequestFriends",
                column: "IdUserIssuer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "RequestFriends");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
