using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieForum.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Data.Configuration
{
    public class MovieActorConfiguration : IEntityTypeConfiguration<MovieActor>
    {
        public void Configure(EntityTypeBuilder<MovieActor> moviceActor)
        {
            moviceActor
                .HasKey(t => new { t.MovieId, t.ActorId });

            //For MovieId in MovieActor table
            moviceActor
                .HasOne(m => m.Movie)
                .WithMany(m => m.Cast)
                .HasForeignKey(m => m.MovieId)
                .OnDelete(DeleteBehavior.NoAction);

            //For ActorId in MovieActor table
            moviceActor.
                HasOne(a => a.Actor)
                .WithMany(a => a.Roles)
                .HasForeignKey(a => a.ActorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
