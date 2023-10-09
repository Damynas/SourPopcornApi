using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "directors",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    country = table.Column<string>(type: "text", nullable: false),
                    bornOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_directors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: false),
                    passwordHash = table.Column<string>(type: "text", nullable: false),
                    displayName = table.Column<string>(type: "text", nullable: false),
                    roles = table.Column<string>(type: "text", nullable: false),
                    createdOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "movies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    directorId = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    country = table.Column<string>(type: "text", nullable: false),
                    language = table.Column<string>(type: "text", nullable: false),
                    releasedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    writers = table.Column<string>(type: "text", nullable: false),
                    actors = table.Column<string>(type: "text", nullable: false),
                    createdOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_movies", x => x.id);
                    table.ForeignKey(
                        name: "fK_movies_directors_directorId",
                        column: x => x.directorId,
                        principalTable: "directors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ratings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    movieId = table.Column<int>(type: "integer", nullable: false),
                    creatorId = table.Column<int>(type: "integer", nullable: false),
                    sourPopcorns = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: false),
                    createdOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_ratings", x => x.id);
                    table.ForeignKey(
                        name: "fK_ratings_movies_movieId",
                        column: x => x.movieId,
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_ratings_users_creatorId",
                        column: x => x.creatorId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "votes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ratingId = table.Column<int>(type: "integer", nullable: false),
                    creatorId = table.Column<int>(type: "integer", nullable: false),
                    isPositive = table.Column<bool>(type: "boolean", nullable: false),
                    createdOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_votes", x => x.id);
                    table.ForeignKey(
                        name: "fK_votes_ratings_ratingId",
                        column: x => x.ratingId,
                        principalTable: "ratings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_votes_users_creatorId",
                        column: x => x.creatorId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "iX_movies_directorId",
                table: "movies",
                column: "directorId");

            migrationBuilder.CreateIndex(
                name: "iX_ratings_creatorId",
                table: "ratings",
                column: "creatorId");

            migrationBuilder.CreateIndex(
                name: "iX_ratings_movieId",
                table: "ratings",
                column: "movieId");

            migrationBuilder.CreateIndex(
                name: "iX_votes_creatorId",
                table: "votes",
                column: "creatorId");

            migrationBuilder.CreateIndex(
                name: "iX_votes_ratingId",
                table: "votes",
                column: "ratingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "votes");

            migrationBuilder.DropTable(
                name: "ratings");

            migrationBuilder.DropTable(
                name: "movies");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "directors");
        }
    }
}
