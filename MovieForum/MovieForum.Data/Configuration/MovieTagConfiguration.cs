using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieForum.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Data.Configuration
{
    public class MovieTagConfiguration : IEntityTypeConfiguration<MovieTags>
    {
        public void Configure(EntityTypeBuilder<MovieTags> moviceTags)
        {
            moviceTags
               .HasKey(t => new { t.MovieId, t.TagId });

            //For MovieId in MovieTags table
            moviceTags
                .HasOne(m => m.Movie)
                .WithMany(m => m.Tags)
                .HasForeignKey(m => m.MovieId)
                .OnDelete(DeleteBehavior.NoAction);

            //For TagId in MovieTags table
            moviceTags
                .HasOne(a => a.Tag)
                .WithMany(a => a.Movies)
                .HasForeignKey(a => a.TagId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
