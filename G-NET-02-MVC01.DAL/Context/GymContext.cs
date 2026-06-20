using G_NET_02_MVC01.DAL.Configuration;
using G_NET_02_MVC01.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using System.Text;

namespace G_NET_02_MVC01.DAL.Context
{
    public class GymContext : DbContext
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
            
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Plans> plans { get; set; }
    }
}
