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
                    Name = "Comedy"
                },
                new Genre
                {
                    Id = 2,
                    Name = "Sci-Fi"
                },
                new Genre
                {
                    Id = 3,
                    Name = "Horror"
                },
                new Genre
                {
                    Id = 4,
                    Name = "Romance"
                },
                new Genre
                {
                    Id = 5,
                    Name = "Action"
                },
                new Genre
                {
                    Id = 6,
                    Name = "Thriller"
                },
                new Genre
                {
                    Id = 7,
                    Name = "Drama"
                },
                new Genre
                {
                    Id = 8,
                    Name = "Mystery"
                },
                new Genre
                {
                    Id = 9,
                    Name = "Crime"
                },
                new Genre
                {
                    Id = 10,
                    Name = "Animation"
                },
                new Genre
                {
                    Id = 11,
                    Name = "Adventure"
                },
                new Genre
                {
                    Id = 12,
                    Name = "Fantasy"
                },
                new Genre
                {
                    Id = 13,
                    Name = "Superhero"
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
                    Content = "Pulna Boza",
                    AuthorId = 1,
                    MovieId = 1,
                    PostedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new Comment
                {
                    Id = 2,
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
                    Title = "Top Gun the new one",
                    Content = "On of my favourite movies of all time",
                    GenreId = 5,
                    ReleaseDate = DateTime.Now,
                },
                new Movie
                {
                    Id = 2,
                    AuthorID = 2,
                    Title = "Spiderman: Far From Home",
                    Content = "The bes spiderman movie so far, I love Tom Holand",
                    GenreId = 13,
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
                    IsDeleted = false,
                },
                new Actor
                {
                    Id=2,
                    FirstName = "Tom",
                    LastName = "Holand",
                    IsDeleted = false,

                }
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

                    MovieId = 1,
                    ActorId = 2,
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
                    Rate = 5
                    
                },
                new Rating
                {
                    Id=2,
                    UserID = 2,
                    MovieId = 2,
                    Rate = 7
                }

                
            };
            db.Entity<Rating>().HasData(ratings);

        }



    }
}
