using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicDistro.Core.Entities;
using MusicDistro.Core.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDistory.Data.Configurations
{
    class AuditConfiguration : IEntityTypeConfiguration<UserAudit>
    {
        public void Configure(EntityTypeBuilder<UserAudit> builder)
        {
            builder
                .ToTable("UserAudits")
                .HasKey(a => a.Id);

            // Creating a navigation property
            builder
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId);
        }
    }
}
