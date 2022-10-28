﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieForum.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.CheckConstraint("CK_Tags_TagName", "(LEN(TagName))>= 2");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.CheckConstraint("CK_Users_Username", "(LEN(Username))>= 4");
                    table.CheckConstraint("CK_Users_FirstName", "(LEN(FirstName))>= 4");
                    table.CheckConstraint("CK_Users_LastName", "(LEN(LastName))>= 4");
                    table.CheckConstraint("CK_Users_Password", "(LEN(Password))>= 8");
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Posted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                    table.CheckConstraint("CK_Movies_Title", "(LEN(Title))>= 2");
                    table.CheckConstraint("CK_Movies_Content", "(LEN(Content))>= 32");
                    table.ForeignKey(
                        name: "FK_Movies_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movies_Users_AuthorID",
                        column: x => x.AuthorID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    LikesCount = table.Column<int>(type: "int", nullable: false),
                    DisLikesCount = table.Column<int>(type: "int", nullable: false),
                    PostedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.CheckConstraint("CK_Comments_Content", "(LEN(Content)) >= 10");
                    table.ForeignKey(
                        name: "FK_Comments_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MovieActors",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    ActorId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieActors", x => new { x.MovieId, x.ActorId });
                    table.ForeignKey(
                        name: "FK_MovieActors_Actors_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MovieActors_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MoviesTags",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesTags", x => new { x.MovieId, x.TagId });
                    table.ForeignKey(
                        name: "FK_MoviesTags_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MoviesTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rating_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LikesDislikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Liked = table.Column<bool>(type: "bit", nullable: false),
                    Disliked = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CommentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikesDislikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LikesDislikes_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "Id", "DeletedOn", "FirstName", "IsDeleted", "LastName" },
                values: new object[,]
                {
                    { 1, null, "Tom", false, "Cruize" },
                    { 2, null, "Tom", false, "Holand" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "DeletedOn", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 13, null, false, "Superhero" },
                    { 12, null, false, "Fantasy" },
                    { 11, null, false, "Adventure" },
                    { 10, null, false, "Animation" },
                    { 9, null, false, "Crime" },
                    { 7, null, false, "Drama" },
                    { 8, null, false, "Mystery" },
                    { 5, null, false, "Action" },
                    { 4, null, false, "Romance" },
                    { 3, null, false, "Horror" },
                    { 2, null, false, "Sci-Fi" },
                    { 1, null, false, "Comedy" },
                    { 6, null, false, "Thriller" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "DeletedOn", "IsDeleted", "TagName" },
                values: new object[,]
                {
                    { 1, null, false, "drama" },
                    { 2, null, false, "action" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DeletedOn", "Email", "FirstName", "ImagePath", "IsBlocked", "IsDeleted", "LastName", "Password", "PhoneNumber", "RoleId", "Username" },
                values: new object[] { 2, null, "adminsemail@gmail.com", "Maggie", "Images/Image_500.jpg", false, false, "TheBoss", "AQAAAAEAACcQAAAAECI28iQIJufP3Kn6hT90n87hw+r774rkIgLfXIRPigBLakUDwVT3kWHlb3rPpLEPDA==", null, 1, "Maggie" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DeletedOn", "Email", "FirstName", "ImagePath", "IsBlocked", "IsDeleted", "LastName", "Password", "PhoneNumber", "RoleId", "Username" },
                values: new object[] { 3, null, "morefakeemails@gmail.com", "Radoslav", "Images/Image_500.jpg", false, false, "Berov", "AQAAAAEAACcQAAAAEBjQvP356RT+khWuIxR+HYGs9zv58caUixUDseCGyRX8czMndmEErSVo6EQ1HIXlLg==", null, 1, "Rado561" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DeletedOn", "Email", "FirstName", "ImagePath", "IsBlocked", "IsDeleted", "LastName", "Password", "PhoneNumber", "RoleId", "Username" },
                values: new object[] { 1, null, "fakeemail@gmail.com", "Angel", "Images/Image_500.jpg", false, false, "Marinski", "AQAAAAEAACcQAAAAEN5vZcJ1fRhWV9IRPjpExVoLQ+rK93UdXcygHMC5O/23r9VnUygmZFAi4pPjM8z3MA==", null, 2, "AngelMarinski" });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "AuthorID", "Content", "DeletedOn", "GenreId", "ImagePath", "IsDeleted", "Posted", "ReleaseDate", "Title" },
                values: new object[] { 2, 2, "The bes spiderman movie so far, I love Tom Holand", null, 13, "Images/Image_500.jpg", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 10, 28, 13, 53, 55, 101, DateTimeKind.Local).AddTicks(1855), "Spiderman: Far From Home" });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "AuthorID", "Content", "DeletedOn", "GenreId", "ImagePath", "IsDeleted", "Posted", "ReleaseDate", "Title" },
                values: new object[] { 1, 1, "On of my favourite movies of all time", null, 5, "Images/Image_500.jpg", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 10, 28, 13, 53, 55, 101, DateTimeKind.Local).AddTicks(1233), "Top Gun the new one" });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "AuthorId", "Content", "DeletedOn", "DisLikesCount", "IsDeleted", "LikesCount", "MovieId", "PostedOn" },
                values: new object[,]
                {
                    { 2, 3, "unikalna produkciq siujeta e ubiec", null, 0, false, 0, 2, new DateTime(2022, 10, 28, 13, 53, 55, 100, DateTimeKind.Local).AddTicks(8139) },
                    { 1, 1, "Pulna Boza", null, 0, false, 0, 1, new DateTime(2022, 10, 28, 13, 53, 55, 98, DateTimeKind.Local).AddTicks(2659) }
                });

            migrationBuilder.InsertData(
                table: "MovieActors",
                columns: new[] { "ActorId", "MovieId", "DeletedOn", "IsDeleted" },
                values: new object[,]
                {
                    { 1, 1, null, false },
                    { 2, 1, null, false }
                });

            migrationBuilder.InsertData(
                table: "MoviesTags",
                columns: new[] { "MovieId", "TagId", "DeletedOn", "IsDeleted" },
                values: new object[,]
                {
                    { 2, 1, null, false },
                    { 1, 2, null, false }
                });

            migrationBuilder.InsertData(
                table: "Rating",
                columns: new[] { "Id", "DeletedOn", "IsDeleted", "MovieId", "Rate", "UserID" },
                values: new object[,]
                {
                    { 1, null, false, 2, 5, 1 },
                    { 2, null, false, 2, 7, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MovieId",
                table: "Comments",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_LikesDislikes_CommentId",
                table: "LikesDislikes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieActors_ActorId",
                table: "MovieActors",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_AuthorID",
                table: "Movies",
                column: "AuthorID");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_GenreId",
                table: "Movies",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesTags_TagId",
                table: "MoviesTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_MovieId",
                table: "Rating",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikesDislikes");

            migrationBuilder.DropTable(
                name: "MovieActors");

            migrationBuilder.DropTable(
                name: "MoviesTags");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}