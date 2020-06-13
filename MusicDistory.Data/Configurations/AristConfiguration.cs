using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicDistro.Core.Entities;

namespace MusicDistory.Data.Configurations
{
    class AristConfiguration : IEntityTypeConfiguration<Artist>
    {
        public void Configure(EntityTypeBuilder<Artist> builder)
        {
            builder
                .ToTable("Artists")
                .HasKey(a => a.Id);

            builder
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
