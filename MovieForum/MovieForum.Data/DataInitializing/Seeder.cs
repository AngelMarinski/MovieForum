using Microsoft.EntityFrameworkCore;
using MovieForum.Data.Models;
using MovieForum.Models;
using System;
using System.Collections.Generic;
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
                        IsDeleted = false
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
                        IsDeleted = false
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
                        IsDeleted = false
                    }
            };

            db.Entity<User>().HasData(users);

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

            var movietags = new List<MovieTags>()
            {
                new MovieTags
                {
                    Id = 1,
                    MovieId = 1,
                    TagId = 2,
                },
                new MovieTags
                {
                    Id = 2,
                    MovieId = 2,
                    TagId = 1,
                }
            };

            db.Entity<MovieTags>().HasData(movietags);

            var tags = new List<Tag>()
            {
                new Tag
                {
                    Id = 1,
                    TagName = "drama",
                    Movies = new List<MovieTags>()
                    {
                        movietags[1]
                    },
                    IsDeleted = false
                },
                new Tag
                {
                    Id = 2,
                    TagName = "action",
                    Movies = new List<MovieTags>()
                    {
                        movietags[0]
                    },
                    IsDeleted = false
                }
            };

            db.Entity<Tag>().HasData(tags);

            var movieActors = new List<MovieActor>()
            {
                new MovieActor
                {
                    Id = 1,
                    MovieId = 1,
                    ActorId = 1
                },
                new MovieActor
                {
                    Id = 2,
                    MovieId = 2,
                    ActorId = 2
                }
            };

            db.Entity<MovieActor>().HasData(movieActors);

            var actors = new List<Actor>()
            {
                new Actor
                {
                    Id=1,
                    FirstName = "Tom",
                    LastName = "Cruize",
                    Roles = new List<MovieActor>()
                    {
                        movieActors[0]
                    }
                },
                new Actor
                {
                    Id=2,
                    FirstName = "Tom",
                    LastName = "Holand",
                    Roles = new List<MovieActor>()
                    {
                        movieActors[1]
                    }
                }
            };

            db.Entity<Actor>().HasData(actors);

            var comments = new List<Comment>()
            {
                new Comment
                {
                    Id = 1,
                    Title = "Ebati tupiq film",
                    Content = "Pulna Boza",
                    UserID = 1,
                    PostedOn = DateTime.Now,
                    IsDeleted = false
                },
                new Comment
                {
                    Id = 2,
                    Title = "Lol mnogo gotino",
                    Content = "unikalna produkciq siujeta e ubiec",
                    UserID = 3,
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
                    Title = "Top Gun",
                    ReleaseDate = DateTime.Now,
                    Cast = new List<MovieActor>()
                    {
                        movieActors[0]
                    },
                    Tags = new List<MovieTags>()
                    {
                        movietags[0]
                    },
                    Comments = new List<Comment>()
                    {
                        comments[0]
                    }
                },
                new Movie
                {
                    Id = 2,
                    AuthorID = 2,
                    Title = "Spiderman: Far From Home",
                    ReleaseDate = DateTime.Now,
                    Cast = new List<MovieActor>()
                    {
                        movieActors[1]
                    },
                    Tags = new List<MovieTags>()
                    {
                        movietags[1]
                    },
                    Comments = new List<Comment>()
                    {
                        comments[1]
                    }
                }
            };
        }
    }
}
