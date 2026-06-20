using G_NET_02_MVC01.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET_02_MVC01.DAL.Configurations
{
    internal class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(X => X.categoryName)
                .HasColumnType("varchar")
                .HasMaxLength(20);

            builder.HasData(
                             new Category { Id = 1, categoryName = "Cardio" },
                             new Category { Id = 2, categoryName = "Strength" },
                             new Category { Id = 3, categoryName = "Yoga" },
                             new Category { Id = 4, categoryName = "Boxing" },
                             new Category { Id = 5, categoryName = "CrossFit" }
                         );

        }
    }
}
