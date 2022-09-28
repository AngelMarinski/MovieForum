using Microsoft.EntityFrameworkCore;
using MovieForum.Data.Models;
using MovieForum.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Data
{
    public class MovieForumContext : DbContext
    {
        public MovieForumContext(DbContextOptions<MovieForumContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        
        public DbSet<Movie> Movies { get; set; }
        
        public DbSet<Comment> Comments { get; set; }
        
        public DbSet<Role> Roles { get; set; }
        
        public DbSet<Actor> Actors { get; set; }

        public DbSet<MovieActor> MovieActors { get; set; }

        public DbSet<MovieTags> MoviesTags { get; set; }

        public DbSet<Tag> Tags { get; set; }
        
        // public DbSet<Genres> Genres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
