using System;
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
                    IsEmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
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
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
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
                    { 13, null, "Chloë ", false, "Moretz" },
                    { 12, null, "Bradley", false, "Cooper" },
                    { 11, null, "Y'Lan", false, "Noel" },
                    { 9, null, "Amber", false, "Midthunder" },
                    { 8, null, "Taylor", false, "Russel" },
                    { 10, null, "George", false, "Clooney" },
                    { 6, null, "Danielle", false, "Panabaker" },
                    { 5, null, "Ethan", false, "Hawke" },
                    { 4, null, "Florence", false, "Pugh" },
                    { 3, null, "Paul", false, "Walker" },
                    { 2, null, "Tom", false, "Holand" },
                    { 7, null, "Ryan", false, "Reynolds" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "DeletedOn", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 9, null, false, "Crime" },
                    { 13, null, false, "Superhero" },
                    { 12, null, false, "Fantasy" },
                    { 11, null, false, "Adventure" },
                    { 10, null, false, "Animation" },
                    { 8, null, false, "Mystery" },
                    { 2, null, false, "Sci-Fi" },
                    { 6, null, false, "Thriller" },
                    { 5, null, false, "Action" },
                    { 4, null, false, "Romance" },
                    { 3, null, false, "Horror" },
                    { 1, null, false, "Comedy" },
                    { 7, null, false, "Drama" }
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
                columns: new[] { "Id", "DeletedOn", "Email", "FirstName", "ImagePath", "IsBlocked", "IsDeleted", "IsEmailConfirmed", "LastName", "Password", "PhoneNumber", "RoleId", "Username" },
                values: new object[,]
                {
                    { 2, null, "adminsemail@gmail.com", "Maggie", "anais.png", false, false, true, "TheBoss", "AQAAAAEAACcQAAAAEK1U7hgGCm7AVi79SDJctcEvcXtQ4qTe/lnCA7IvmrntHcmhxti0aHZ7GO+pobKN6g==", null, 1, "Maggie" },
                    { 3, null, "morefakeemails@gmail.com", "Radoslav", "darw.png", false, false, true, "Berov", "AQAAAAEAACcQAAAAEC8Bql50QiZtA96AF2jmCCybXlg4UVXvtFP5ovJlDnMpIRwOQGBX1J1BlMHdR9EoFw==", null, 1, "Rado561" },
                    { 4, null, "agent007@gmail.com", "James", "bond.png", false, false, true, "Bond", "AQAAAAEAACcQAAAAEMl1597zn60k2rKZ5XzwQzs9wAd2gA8DpnBx0tdP7dR/TbF1gzrK6EaEoSFKeA0nmw==", null, 1, "James96" },
                    { 5, null, "mzfb@gmail.com", "Mark", "marck.jpg", false, false, true, "zuckerberg", "AQAAAAEAACcQAAAAEDFGDEHin70W0dby513su7LaYhFhrLn+zFh+s5JacWDsU4G8f2y7d6f038VFlAhM3Q==", null, 1, "MarkZ" },
                    { 1, null, "fakeemail@gmail.com", "Angel", "gum.jpg", false, false, true, "Marinski", "AQAAAAEAACcQAAAAEBdkZwoIPPzYKnNyaMK0G8LZWfOONPEpjSsSf6USY4+6bd6tSrdj7xoiMblrWc8jRA==", null, 2, "AngelMarinski" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "AuthorID", "Content", "DeletedOn", "GenreId", "ImagePath", "IsDeleted", "Posted", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { 2, 2, "With Spider-Man's identity now revealed, Peter asks Doctor Strange for help. When a spell goes wrong, dangerous foes from other worlds start to appear, forcing Peter to discover what it truly means to be Spider-Man.\r\n\r\nPeter Parker's secret identity is revealed to the entire world. Desperate for help, Peter turns to Doctor Strange to make the world forget that he is Spider-Man. The spell goes horribly wrong and shatters the multiverse, bringing in monstrous villains that could destroy the world.\r\n\r\nPicking up where Far From Home left off, Peter Parker's whole world is turned upside down when his old enemy Mysterio posthumously reveals his identity to the public. Wanting to make his identity a secret, Peter turns to Doctor Strange for help. But when Strange's spell goes haywire, Peter must go up against five deadly new enemies--the Green Goblin, Dr. Octopus, Electro, the Lizard and Sandman--all while discovering what it truly means to be Spider-Man.", null, 13, "Images/spider-man.jpg", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spiderman: No Way Home" },
                    { 7, 2, "Foul-mouthed mutant mercenary Wade Wilson (a.k.a. Deadpool) assembles a team of fellow mutant rogues to protect a young boy with supernatural abilities from the brutal, time-traveling cyborg Cable.\r\n\r\nAfter losing Vanessa (Morena Baccarin), the love of his life, 4th-wall breaking mercenary Wade Wilson aka Deadpool (Ryan Reynolds) must assemble a team and protect a young, fat mutant Russell Collins aka Firefist (Julian Dennison) from Cable (Josh Brolin), a no-nonsense, dangerous cyborg from the future, and must also learn the most important lesson of all: to be part of a family again.", null, 11, "Images/dp2.jpg", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2018, 5, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deadpool 2" },
                    { 3, 3, "Former cop Brian O'Conner is called upon to bust a dangerous criminal and he recruits the help of a former childhood friend and street racer who has a chance to redeem himself.", null, 4, "Images/2f2f.jpg", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2003, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "2 Fast 2 Furious" },
                    { 8, 3, "Six people unwillingly find themselves locked in another series of escape rooms, slowly uncovering what they have in common to survive. Joining forces with two of the original survivors, they soon discover they've all played the game before.\r\n\r\nAfter escaping death by the skin of their teeth and surviving the bloody games devised by the nefarious Minos Corporation, lucky players Zoey and Ben are now coping with trauma after the events of Escape Room (2019). But instead of finding answers and closure, Zoey and Ben are soon dragged into another set of fiendishly designed death traps. And as a handful of former escapees join in, the competitors must work together to outsmart their inhuman tormentors and give their all to solve increasingly sophisticated puzzles. This time, the unseen perils are more elaborate, and escape is futile. Who shall live and who shall die in the sadistic Tournament of Champions?", null, 6, "Images/escape-room.jpg", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Escape Room: Tournament of Champions" },
                    { 9, 3, "Naru, a skilled warrior of the Comanche Nation, fights to protect her tribe against one of the first highly-evolved Predators to land on Earth.", null, 3, "Images/prey.png", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Prey" },
                    { 12, 3, "When one of their own is kidnapped by an angry gangster, the Wolf Pack must track down Mr. Chow, who has escaped from prison and is on the run.", null, 1, "Images/hangover.jpg", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2013, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Hangover Part III" },
                    { 4, 4, "A 1950s housewife living with her husband in a utopian experimental community begins to worry that his glamorous company could be hiding disturbing secrets.", null, 4, "Images/dwd.jpg", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Don't Worry Darling" },
                    { 10, 4, "A divorced couple teams up and travels to Bali to stop their daughter from making the same mistake they think they made 25 years ago.", null, 4, "Images/ticket.jpg", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ticket to Paradise" },
                    { 5, 5, "For his final assignment, a top temporal agent must pursue the one criminal that has eluded him throughout time. The chase turns into a unique, surprising and mind-bending exploration of love, fate, identity and time travel taboos.\r\n\r\nPREDESTINATION chronicles the life of a Temporal Agent sent on an intricate series of time-travel journeys designed to ensure the continuation of his law enforcement career for all eternity. Now, on his final assignment, the Agent must pursue the one criminal that has eluded him throughout time.", null, 8, "Images/predestination.jpg", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2014, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Predestination" },
                    { 11, 5, "America's third political party, the New Founding Fathers of America, comes to power and conducts an experiment: no laws for 12 hours on Staten Island. No one has to stay on the island, but $5,000 is given to anyone who does.\r\n\r\nThis horror/action-adventure film from director Gerard McMurray serves as a prequel that recounts events that led up to the first Purge event. To push the crime rate below one percent for the rest of the year, the New Founding Fathers of America (NFFA) test a sociological theory that vents aggression for one night in one isolated community.", null, 9, "Images/purge.jpg", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2018, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "The First Purge" },
                    { 1, 1, "After more than thirty years of service, military awards, medals and decorations for extraordinary heroism in combat, distinguished US Navy Captain Pete Mitchell, call sign Maverick, finds himself exactly where he belongs: pushing the limits as a top test pilot. Having spent years avoiding promotions after the events of Top Gun (1986), Maverick must now confront the ugly past and an uncertain future while tasked with training the next generation of elite fighter pilots for a nearly impossible suicide mission. But as the veteran naval aviator prepares the brilliant graduates for the top-secret assignment, stretching the rules to the breaking point, Mitchell has to face an equally critical challenge: navigate through an uncomfortable, bitter relationship with a hotshot lieutenant holding a grudge. Can Maverick and his Top Guns perform a miracle, give the enemy hell, and come back home in one piece?", null, 5, "Images/top-gun.jpg", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 5, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Top Gun:Maverick" },
                    { 6, 1, "Three friends discover a mysterious machine that takes pictures twenty-four hours into the future, and conspire to use it for personal gain, until disturbing and dangerous images begin to develop.\r\n\r\nThree friends discover a time machine which takes pictures of the future. They begin to use it to win race bets and everything goes fine till one gets greedier than another. They begin to lose faith in each other giving a sense of backstabbing as uglier truths unfold in the photos and the situation soon gets out of control.", null, 8, "Images/timelapse.jpg", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2014, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Time Lapse" },
                    { 13, 1, "Set in the future when technology has subtly altered society, a woman discovers a secret connection to an alternate reality as well as a dark future of her own.", null, 2, "Images/periph.jpg", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Peripheral" }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "AuthorId", "Content", "DeletedOn", "DisLikesCount", "IsDeleted", "LikesCount", "MovieId", "PostedOn" },
                values: new object[,]
                {
                    { 2, 2, "Amazing movie! I strongly recomend it!", null, 0, false, 0, 2, new DateTime(2022, 11, 4, 21, 28, 25, 835, DateTimeKind.Local).AddTicks(9302) },
                    { 1, 1, "This movie is awful!", null, 0, false, 0, 1, new DateTime(2022, 11, 4, 21, 28, 25, 832, DateTimeKind.Local).AddTicks(4855) },
                    { 5, 5, "One of the greatest mindblowing movies of all the times!", null, 0, false, 0, 5, new DateTime(2022, 11, 4, 21, 28, 25, 835, DateTimeKind.Local).AddTicks(9371) },
                    { 3, 3, "It's not as good as the first and repeats the first film's plot but 2 Fast 2 Furious is still fun due to great performances from Paul Walker and Tyrese Gibson and their great chemistry. The enjoyable and over the top action helps as well.!", null, 0, false, 0, 3, new DateTime(2022, 11, 4, 21, 28, 25, 835, DateTimeKind.Local).AddTicks(9359) },
                    { 4, 4, "The concept of the movie was good, but needed to be taken a bit further in my opinion with detail. A lot happens in the movie that basically requires the audience to accept without question.\r\n", null, 0, false, 0, 4, new DateTime(2022, 11, 4, 21, 28, 25, 835, DateTimeKind.Local).AddTicks(9366) }
                });

            migrationBuilder.InsertData(
                table: "MovieActors",
                columns: new[] { "ActorId", "MovieId", "DeletedOn", "IsDeleted" },
                values: new object[,]
                {
                    { 1, 1, null, false },
                    { 11, 11, null, false },
                    { 5, 5, null, false },
                    { 10, 10, null, false },
                    { 4, 4, null, false },
                    { 12, 12, null, false },
                    { 9, 9, null, false },
                    { 8, 8, null, false },
                    { 3, 3, null, false },
                    { 7, 7, null, false },
                    { 2, 2, null, false },
                    { 6, 6, null, false },
                    { 13, 13, null, false }
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
                    { 2, null, false, 2, 7, 2 },
                    { 1, null, false, 2, 5, 1 }
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
