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
          

            var comments = new List<Comment>()
            {
                new Comment
                {
                    Id = 1,
                    Title = "Ebati tupiq film",
                    Content = "Pulna Boza",
                    AuthorId = 1,
                    MovieId = 1,
                    PostedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new Comment
                {
                    Id = 2,
                    Title = "Lol mnogo gotino",
                    Content = "unikalna produkciq siujeta e ubiec",
                    AuthorId = 3,
                    MovieId = 2,
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

                },
                new Movie
                {
                    Id = 2,
                    AuthorID = 2,
                    Title = "Spiderman: Far From Home",
                    ReleaseDate = DateTime.Now,

                }
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

                },
                new Actor
                {
                    Id=2,
                    FirstName = "Tom",
                    LastName = "Holand",
                    

                }
            };

            db.Entity<Actor>().HasData(actors);
            var movieActors = new List<MovieActor>()
            {
                new MovieActor
                {
                    MovieId = 1,
                    ActorId = 1
                },
                new MovieActor
                {

                    MovieId = 1,
                    ActorId = 2
                }
            };

            db.Entity<MovieActor>().HasData(movieActors);

            var movietags = new List<MovieTags>()
            {
                new MovieTags
                {
                    MovieId = 1,
                    TagId = 2,
                },
                new MovieTags
                {
                    MovieId = 2,
                    TagId = 1,
                }
            };
            db.Entity<MovieTags>().HasData(movietags);
        }



    }
}
