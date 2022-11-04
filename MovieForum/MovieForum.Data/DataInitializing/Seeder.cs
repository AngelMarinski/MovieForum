using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieForum.Data.Models;
using MovieForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieForum.Data.DataInitializing
{
    public static class Seeder
    {
        public static void Seed(this ModelBuilder db)
        {
            var users = new List<User>()
            {
                 new User
                    {
                        Id = 1,
                        Username = "AngelMarinski",
                        FirstName = "Angel",
                        LastName = "Marinski",
                        Password = "12345678",
                        Email = "fakeemail@gmail.com",
                        RoleId = 2,
                        IsDeleted = false,
                        ImagePath = "gum.jpg",
                        IsEmailConfirmed = true
                    },
                 new User
                    {
                        Id = 2,
                        Username = "Maggie",
                        FirstName = "Maggie",
                        LastName = "TheBoss",
                        Password = "12345678",
                        Email = "adminsemail@gmail.com",
                        RoleId = 1,
                        IsDeleted = false,
                        ImagePath = "anais.png",
                        IsEmailConfirmed = true
                    },
                  new User
                    {
                        Id = 3,
                        Username = "Rado561",
                        FirstName = "Radoslav",
                        LastName = "Berov",
                        Password = "12345678",
                        Email = "morefakeemails@gmail.com",
                        RoleId = 1,
                        IsDeleted = false,
                        ImagePath = "darw.png",
                        IsEmailConfirmed = true
                    },
                   new User
                    {
                        Id = 4,
                        Username = "James96",
                        FirstName = "James",
                        LastName = "Bond",
                        Password = "12345678",
                        Email = "agent007@gmail.com",
                        RoleId = 1,
                        IsDeleted = false,
                        ImagePath = "bond.png",
                        IsEmailConfirmed = true
                    },
                    new User
                    {
                        Id = 5,
                        Username = "MarkZ",
                        FirstName = "Mark",
                        LastName = "zuckerberg",
                        Password = "facebook",
                        Email = "mzfb@gmail.com",
                        RoleId = 1,
                        IsDeleted = false,
                        ImagePath = "marck.jpg",
                        IsEmailConfirmed = true
                    }
            };


            var passwordHasher = new PasswordHasher<User>();
            foreach (var item in users)
            {
                item.Password = passwordHasher.HashPassword(item, item.Password);
            }

            db.Entity<User>().HasData(users);

            var genres = new List<Genre>()
            {
                new Genre
                {
                    Id = 1,
                    Name = "Comedy",
                    IsDeleted = false
                },
                new Genre
                {
                    Id = 2,
                    Name = "Sci-Fi",
                    IsDeleted = false
                },
                new Genre
                {
                    Id = 3,
                    Name = "Horror",
                    IsDeleted = false
                },
                new Genre
                {
                    Id = 4,
                    Name = "Romance",
                    IsDeleted = false
                },
                new Genre
                {
                    Id = 5,
                    Name = "Action",
                    IsDeleted = false
                },
                new Genre
                {
                    Id = 6,
                    Name = "Thriller",
                    IsDeleted = false
                },
                new Genre
                {
                    Id = 7,
                    Name = "Drama",
                    IsDeleted = false
                },
                new Genre
                {
                    Id = 8,
                    Name = "Mystery",
                    IsDeleted = false
                },
                new Genre
                {
                    Id = 9,
                    Name = "Crime",
                    IsDeleted = false
                },
                new Genre
                {
                    Id = 10,
                    Name = "Animation",
                    IsDeleted = false
                },
                new Genre
                {
                    Id = 11,
                    Name = "Adventure",
                    IsDeleted = false
                },
                new Genre
                {
                    Id = 12,
                    Name = "Fantasy",
                    IsDeleted = false
                },
                new Genre
                {
                    Id = 13,
                    Name = "Superhero",
                    IsDeleted = false
                }
            };

            db.Entity<Genre>().HasData(genres);

            var roles = new List<Role>()
            {
                new Role
                {
                    Id = 1,
                    Name = "Admin"
                },
                new Role
                {
                    Id = 2,
                    Name = "User"
                }
            };

            db.Entity<Role>().HasData(roles);


            var comments = new List<Comment>()
            {
                new Comment
                {
                    Id = 1,
                    Content = "This movie is awful!",
                    AuthorId = 1,
                    MovieId = 1,
                    PostedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new Comment
                {
                    Id = 2,
                    Content = "Amazing movie! I strongly recomend it!",
                    AuthorId = 2,
                    MovieId = 2,
                    PostedOn = DateTime.Now,
                    IsDeleted = false
                },
                new Comment
                {
                    Id = 3,
                    Content = "It's not as good as the first and repeats the first film's plot but 2 Fast 2 Furious is still fun due to great performances from Paul Walker and Tyrese Gibson and their great chemistry. The enjoyable and over the top action helps as well.!",
                    AuthorId = 3,
                    MovieId = 3,
                    PostedOn = DateTime.Now,
                    IsDeleted = false
                },
                new Comment
                {
                    Id = 4,
                    Content = "The concept of the movie was good, but needed to be taken a bit further in my opinion with detail. A lot happens in the movie that basically requires the audience to accept without question.\r\n",
                    AuthorId = 4,
                    MovieId = 4,
                    PostedOn = DateTime.Now,
                    IsDeleted = false
                },
                new Comment
                {
                    Id = 5,
                    Content = "One of the greatest mindblowing movies of all the times!",
                    AuthorId = 5,
                    MovieId = 5,
                    PostedOn = DateTime.Now,
                    IsDeleted = false
                }
            };

            db.Entity<Comment>().HasData(comments);

            var movies = new List<Movie>()
            {
                new Movie
                {
                    Id = 1,
                    AuthorID = 1,
                    Title = "Top Gun:Maverick",
                    Content = "After more than thirty years of service, " +
                    "military awards, medals and decorations for extraordinary heroism in combat," +
                    " distinguished US Navy Captain Pete Mitchell, call sign Maverick, finds himself" +
                    " exactly where he belongs: pushing the limits as a top test pilot. Having spent years" +
                    " avoiding promotions after the events of Top Gun (1986), Maverick must now confront the ugly" +
                    " past and an uncertain future while tasked with training the next generation of elite fighter " +
                    "pilots for a nearly impossible suicide mission. But as the veteran naval aviator prepares the" +
                    " brilliant graduates for the top-secret assignment, stretching the rules to the breaking point," +
                    " Mitchell has to face an equally critical challenge: navigate through an uncomfortable, " +
                    "bitter relationship with a hotshot lieutenant holding a grudge. Can Maverick and his " +
                    "Top Guns perform a miracle, give the enemy hell, and come back home in one piece?",
                    GenreId = 5,
                    ReleaseDate = new DateTime(2022,05,27),
                    ImagePath = "Images/top-gun.jpg"
                },
                new Movie
                {
                    Id = 2,
                    AuthorID = 2,
                    Title = "Spiderman: No Way Home",
                    Content = "With Spider-Man's identity now revealed, Peter asks Doctor Strange for help. When a spell goes wrong, dangerous foes from other worlds start to appear, forcing Peter to discover what it truly means to be Spider-Man.\r\n\r\nPeter Parker's secret identity is revealed to the entire world. Desperate for help, Peter turns to Doctor Strange to make the world forget that he is Spider-Man. The spell goes horribly wrong and shatters the multiverse, bringing in monstrous villains that could destroy the world.\r\n\r\nPicking up where Far From Home left off, Peter Parker's whole world is turned upside down when his old enemy Mysterio posthumously reveals his identity to the public. Wanting to make his identity a secret, Peter turns to Doctor Strange for help. But when Strange's spell goes haywire, Peter must go up against five deadly new enemies--the Green Goblin, Dr. Octopus, Electro, the Lizard and Sandman--all while discovering what it truly means to be Spider-Man.",
                    GenreId = 13,
                    ReleaseDate = new DateTime(2021,12,17),
                    ImagePath = "Images/spider-man.jpg"

                },
                new Movie
                {
                    Id = 3,
                    AuthorID = 3,
                    Title = "2 Fast 2 Furious",
                    Content = "Former cop Brian O'Conner is called upon to bust a dangerous criminal and he recruits the help of a former childhood friend and street racer who has a chance to redeem himself.",
                    GenreId = 4,
                    ReleaseDate = new DateTime(2003,06,06),
                    ImagePath = "Images/2f2f.jpg"

                },
                new Movie
                {
                    Id = 4,
                    AuthorID = 4,
                    Title = "Don't Worry Darling",
                    Content = "A 1950s housewife living with her husband in a utopian experimental community begins to worry that his glamorous company could be hiding disturbing secrets.",
                    GenreId = 4,
                    ReleaseDate = new DateTime(2022,09,23),
                    ImagePath = "Images/dwd.jpg"

                },
                new Movie
                {
                    Id = 5,
                    AuthorID = 5,
                    Title = "Predestination",
                    Content = "For his final assignment, a top temporal agent must pursue the one criminal that has eluded him throughout time. The chase turns into a unique, surprising and mind-bending exploration of love, fate, identity and time travel taboos.\r\n\r\nPREDESTINATION chronicles the life of a Temporal Agent sent on an intricate series of time-travel journeys designed to ensure the continuation of his law enforcement career for all eternity. Now, on his final assignment, the Agent must pursue the one criminal that has eluded him throughout time.",
                    GenreId = 8,
                    ReleaseDate = new DateTime(2014,08,28),
                    ImagePath = "Images/predestination.jpg"

                },
                new Movie
                {
                    Id = 6,
                    AuthorID = 1,
                    Title = "Time Lapse",
                    Content = "Three friends discover a mysterious machine that takes pictures twenty-four hours into the future, and conspire to use it for personal gain, until disturbing and dangerous images begin to develop.\r\n\r\nThree friends discover a time machine which takes pictures of the future. They begin to use it to win race bets and everything goes fine till one gets greedier than another. They begin to lose faith in each other giving a sense of backstabbing as uglier truths unfold in the photos and the situation soon gets out of control.",
                    GenreId = 8,
                    ReleaseDate = new DateTime(2014,04,18),
                    ImagePath = "Images/timelapse.jpg"

                },
                new Movie
                {
                    Id = 7,
                    AuthorID = 2,
                    Title = "Deadpool 2",
                    Content = "Foul-mouthed mutant mercenary Wade Wilson (a.k.a. Deadpool) assembles a team of fellow mutant rogues to protect a young boy with supernatural abilities from the brutal, time-traveling cyborg Cable.\r\n\r\nAfter losing Vanessa (Morena Baccarin), the love of his life, 4th-wall breaking mercenary Wade Wilson aka Deadpool (Ryan Reynolds) must assemble a team and protect a young, fat mutant Russell Collins aka Firefist (Julian Dennison) from Cable (Josh Brolin), a no-nonsense, dangerous cyborg from the future, and must also learn the most important lesson of all: to be part of a family again.",
                    GenreId = 11,
                    ReleaseDate = new DateTime(2018,05,18),
                    ImagePath = "Images/dp2.jpg"

                },
                new Movie
                {
                    Id = 8,
                    AuthorID = 3,
                    Title = "Escape Room: Tournament of Champions",
                    Content = "Six people unwillingly find themselves locked in another series of escape rooms, slowly uncovering what they have in common to survive. Joining forces with two of the original survivors, they soon discover they've all played the game before.\r\n\r\nAfter escaping death by the skin of their teeth and surviving the bloody games devised by the nefarious Minos Corporation, lucky players Zoey and Ben are now coping with trauma after the events of Escape Room (2019). But instead of finding answers and closure, Zoey and Ben are soon dragged into another set of fiendishly designed death traps. And as a handful of former escapees join in, the competitors must work together to outsmart their inhuman tormentors and give their all to solve increasingly sophisticated puzzles. This time, the unseen perils are more elaborate, and escape is futile. Who shall live and who shall die in the sadistic Tournament of Champions?",
                    GenreId = 6,
                    ReleaseDate = new DateTime(2021,07,16),
                    ImagePath = "Images/escape-room.jpg"

                },
                new Movie
                {
                    Id = 9,
                    AuthorID = 3,
                    Title = "Prey",
                    Content = "Naru, a skilled warrior of the Comanche Nation, fights to protect her tribe against one of the first highly-evolved Predators to land on Earth.",
                    GenreId = 3,
                    ReleaseDate = new DateTime(2022,07,21),
                    ImagePath = "Images/prey.png"

                },
                new Movie
                {
                    Id = 10,
                    AuthorID = 4,
                    Title = "Ticket to Paradise",
                    Content = "A divorced couple teams up and travels to Bali to stop their daughter from making the same mistake they think they made 25 years ago.",
                    GenreId = 4,
                    ReleaseDate = new DateTime(2022,09,20),
                    ImagePath = "Images/ticket.jpg"

                },
                new Movie
                {
                    Id = 11,
                    AuthorID = 5,
                    Title = "The First Purge",
                    Content = "America's third political party, the New Founding Fathers of America, comes to power and conducts an experiment: no laws for 12 hours on Staten Island. No one has to stay on the island, but $5,000 is given to anyone who does.\r\n\r\nThis horror/action-adventure film from director Gerard McMurray serves as a prequel that recounts events that led up to the first Purge event. To push the crime rate below one percent for the rest of the year, the New Founding Fathers of America (NFFA) test a sociological theory that vents aggression for one night in one isolated community.",
                    GenreId = 9,
                    ReleaseDate = new DateTime(2018,07,04),
                    ImagePath = "Images/purge.jpg"

                },
                new Movie
                {
                    Id = 12,
                    AuthorID = 3,
                    Title = "The Hangover Part III",
                    Content = "When one of their own is kidnapped by an angry gangster, the Wolf Pack must track down Mr. Chow, who has escaped from prison and is on the run.",
                    GenreId = 1,
                    ReleaseDate = new DateTime(2013,05,31),
                    ImagePath = "Images/hangover.jpg"

                },
                new Movie
                {
                    Id = 13,
                    AuthorID = 1,
                    Title = "The Peripheral",
                    Content = "Set in the future when technology has subtly altered society, a woman discovers a secret connection to an alternate reality as well as a dark future of her own.",
                    GenreId = 2,
                    ReleaseDate = new DateTime(2022,10,21),
                    ImagePath = "Images/periph.jpg"

                },
            };
            db.Entity<Movie>().HasData(movies);


            var tags = new List<Tag>()
            {
                new Tag
                {
                    Id = 1,
                    TagName = "drama",
                    IsDeleted = false
                },
                new Tag
                {
                    Id = 2,
                    TagName = "action",

                    IsDeleted = false
                }
            };

            db.Entity<Tag>().HasData(tags);


            var actors = new List<Actor>()
            {
                new Actor
                {
                    Id=1,
                    FirstName = "Tom",
                    LastName = "Cruize",
                    IsDeleted = false,
                },
                new Actor
                {
                    Id=2,
                    FirstName = "Tom",
                    LastName = "Holand",
                    IsDeleted = false,

                },
                new Actor
                {
                    Id=3,
                    FirstName = "Paul",
                    LastName = "Walker",
                    IsDeleted = false,

                },
                new Actor
                {
                    Id=4,
                    FirstName = "Florence",
                    LastName = "Pugh",
                    IsDeleted = false,

                },
                new Actor
                {
                    Id=5,
                    FirstName = "Ethan",
                    LastName = "Hawke",
                    IsDeleted = false,

                },
                new Actor
                {
                    Id=6,
                    FirstName = "Danielle",
                    LastName = "Panabaker",
                    IsDeleted = false,

                },
                new Actor
                {
                    Id=7,
                    FirstName = "Ryan",
                    LastName = "Reynolds",
                    IsDeleted = false,

                },
                new Actor
                {
                    Id=8,
                    FirstName = "Taylor",
                    LastName = "Russel",
                    IsDeleted = false,

                },
                new Actor
                {
                    Id=9,
                    FirstName = "Amber",
                    LastName = "Midthunder",
                    IsDeleted = false,

                },
                new Actor
                {
                    Id=10,
                    FirstName = "George",
                    LastName = "Clooney",
                    IsDeleted = false,

                },
                new Actor
                {
                    Id=11,
                    FirstName = "Y'Lan",
                    LastName = "Noel",
                    IsDeleted = false,

                },
                new Actor
                {
                    Id=12,
                    FirstName = "Bradley",
                    LastName = "Cooper",
                    IsDeleted = false,

                },
                new Actor
                {
                    Id=13,
                    FirstName = "Chloë ",
                    LastName = "Moretz",
                    IsDeleted = false,

                },

            };

            db.Entity<Actor>().HasData(actors);

            var movieActors = new List<MovieActor>()
            {
                new MovieActor
                {
                    MovieId = 1,
                    ActorId = 1,
                    IsDeleted = false,
                },
                new MovieActor
                {

                    MovieId = 2,
                    ActorId = 2,
                    IsDeleted = false,
                },
                 new MovieActor
                {

                    MovieId = 3,
                    ActorId = 3,
                    IsDeleted = false,
                },
                 new MovieActor
                {

                    MovieId = 4,
                    ActorId = 4,
                    IsDeleted = false,
                },
                 new MovieActor
                {

                    MovieId = 5,
                    ActorId = 5,
                    IsDeleted = false,
                },
                 new MovieActor
                {

                    MovieId = 6,
                    ActorId = 6,
                    IsDeleted = false,
                },
                 new MovieActor
                {

                    MovieId = 7,
                    ActorId = 7,
                    IsDeleted = false,
                },
                 new MovieActor
                {

                    MovieId = 8,
                    ActorId = 8,
                    IsDeleted = false,
                },
                 new MovieActor
                {

                    MovieId = 9,
                    ActorId = 9,
                    IsDeleted = false,
                },
                 new MovieActor
                {

                    MovieId = 10,
                    ActorId = 10,
                    IsDeleted = false,
                },
                 new MovieActor
                {

                    MovieId = 11,
                    ActorId = 11,
                    IsDeleted = false,
                },
                 new MovieActor
                {

                    MovieId = 12,
                    ActorId = 12,
                    IsDeleted = false,
                },
                 new MovieActor
                {

                    MovieId = 13,
                    ActorId = 13,
                    IsDeleted = false,
                }

            };

            db.Entity<MovieActor>().HasData(movieActors);

            var movietags = new List<MovieTags>()
            {
                new MovieTags
                {
                    MovieId = 1,
                    TagId = 2,
                    IsDeleted = false
                },
                new MovieTags
                {
                    MovieId = 2,
                    TagId = 1,
                    IsDeleted = false
                }
            };
            db.Entity<MovieTags>().HasData(movietags);

            var ratings = new List<Rating>()
            {
                new Rating
                {
                    Id = 1,
                    UserID = 1,
                    MovieId = 2,
                    Rate = 5,
                    IsDeleted = false
                },
                new Rating
                {
                    Id=2,
                    UserID = 2,
                    MovieId = 2,
                    Rate = 7,
                    IsDeleted = false
                }


            };
            db.Entity<Rating>().HasData(ratings);

        }



    }
}
