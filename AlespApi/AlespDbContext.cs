﻿using Alesp.Shared;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

public class AlespDbContext : DbContext
{
        public AlespDbContext(DbContextOptions<AlespDbContext> options) : base(options)
        {
        
        }
        
        public AlespDbContext()
        {

        }

        public DbSet<CongressPerson> CongressPeople { get; set; }
        public DbSet<Legislature> Legislatures { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Spending> Spendings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Legislature>()
                    .HasIndex(p => new { p.Number})
                    .IsUnique(true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            options.UseSqlServer(configuration.GetConnectionString("Default"));
        }

}