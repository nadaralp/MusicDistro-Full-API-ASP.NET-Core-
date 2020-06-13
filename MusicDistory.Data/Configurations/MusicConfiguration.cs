using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicDistro.Core.Entities;

namespace MusicDistory.Data.Configurations
{
    class MusicConfiguration : IEntityTypeConfiguration<Music>
    {
        public void Configure(EntityTypeBuilder<Music> builder)
        {
            // Model Configuration ( model to database )
                // pass


            // Entity Configuration ( model to table )
            builder
                .ToTable("Musics")
                .HasKey(m => m.Id);

            builder
                .HasOne(m => m.Artist)
                .WithMany(a => a.Musics)
                .HasForeignKey(m => m.ArtistId)
                .OnDelete(DeleteBehavior.Cascade);


            // Property Configuration ( model to column )
            builder
                .Property(m => m.Id)
                .UseIdentityColumn();

            builder
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(150);
        }
    }
}
