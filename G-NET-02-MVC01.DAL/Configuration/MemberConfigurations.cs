using G_NET_02_MVC01.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET_02_MVC01.DAL.Configurations
{
    internal class MemberConfigurations : GymUserConfigurations<Member>, IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(X => X.CreatedAt)
                   .HasColumnName("JoinDate")
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(M => M.HealthRecord)
                   .WithOne(HR => HR.Member)
                   .HasForeignKey<HealthRecord>(M => M.MemberId);

            base.Configure(builder);

        }
    }
}
