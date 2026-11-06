using G_NET_02_MVC01.DAL.Configuration;
using G_NET_02_MVC01.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using System.Text;

namespace G_NET_02_MVC01.DAL.Context
{
    public class GymContext : IdentityDbContext<ApplicationUser>
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;database=Gym;trusted_connection=true;trustServerCertificate=true");
        //}
        public GymContext(DbContextOptions<GymContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<ApplicationUser>(ETP => {
                ETP.Property(u => u.FirstName).HasMaxLength(50).HasColumnType("nvarchar(50)");
                ETP.Property(u => u.LastName).HasMaxLength(50).HasColumnType("nvarchar(50)");
            });
        }
        public DbSet<Plans> plans { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<Trainer> Trainer { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<HealthRecord> HealthRecord { get; set; }


}
}
