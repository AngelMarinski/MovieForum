using Microsoft.EntityFrameworkCore;
using MovieForum.Data.DataInitializing;
using MovieForum.Data.Models;
using MovieForum.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Reaction> LikesDislikes { get; set; }

        public DbSet<Rating> Rating { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            modelBuilder.Seed();

            SetMinLengthConstraints(modelBuilder);         



        }

        private static void SetMinLengthConstraints(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            entity.HasCheckConstraint("CK_Comments_Content", "(LEN(Content)) >= 10"));

            modelBuilder.Entity<User>(entity =>
            entity.HasCheckConstraint("CK_Users_Username", "(LEN(Username))>= 4"));

            modelBuilder.Entity<User>(entity =>
            entity.HasCheckConstraint("CK_Users_FirstName", "(LEN(FirstName))>= 4"));

            modelBuilder.Entity<User>(entity =>
            entity.HasCheckConstraint("CK_Users_LastName", "(LEN(LastName))>= 4"));

            modelBuilder.Entity<User>(entity =>
            entity.HasCheckConstraint("CK_Users_Password", "(LEN(Password))>= 8"));

            modelBuilder.Entity<Movie>(entity =>
            entity.HasCheckConstraint("CK_Movies_Title", "(LEN(Title))>= 2"));

            modelBuilder.Entity<Movie>(entity =>
            entity.HasCheckConstraint("CK_Movies_Content", "(LEN(Content))>= 32"));

            modelBuilder.Entity<Tag>(entity =>
            entity.HasCheckConstraint("CK_Tags_TagName", "(LEN(TagName))>= 2"));
        }
    

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        public override int SaveChanges()
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        break;
                }
            }
        }
    }
}
