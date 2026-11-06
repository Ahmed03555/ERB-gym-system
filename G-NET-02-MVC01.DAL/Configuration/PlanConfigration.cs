using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Microsoft.EntityFrameworkCore;
using G_NET_02_MVC01.DAL.Entities;

namespace G_NET_02_MVC01.DAL.Configuration
{
    public class PlanConfigration : IEntityTypeConfiguration<Plans>
    {
        public void Configure(EntityTypeBuilder<Plans> builder) 
        {
            builder.Property(X => X.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50);

            builder.Property(X => X.Description)
                   .HasMaxLength(200);

            builder.Property(X => X.Price)
                .HasPrecision(10, 2);

            builder.Property(X => X.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("PlanDurationCheck",
                    "DurationDays Between 1 and 365");
            });
        }
    }
}
