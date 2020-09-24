using DrugTimer.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrugTimer.Server.Domain.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<DrugInfo> drugInfos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* DRUG INFO TABLE */
            builder.Entity<DrugInfo>().ToTable("DrugInfo");
            builder.Entity<DrugInfo>().HasKey(p => p.Id);

            builder.Entity<DrugInfo>().Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Entity<DrugInfo>().Property(p => p.Name).IsRequired();
            builder.Entity<DrugInfo>().Property(p => p.TimeBetweenDoses).IsRequired();
            builder.Entity<DrugInfo>().HasMany(p => p.DrugEntries).WithOne(p => p.DrugInfo).HasForeignKey(p => p.DrugInfoId);

            //temp hardcoded data
            builder.Entity<DrugInfo>().HasData
            (
                new DrugInfo { Id = 1, Name = "Codeine", TimeBetweenDoses = 4 }
            );

            /* DRUG ENTRIES TABLE */
            builder.Entity<DrugEntries>().ToTable("DrugEntries");
            builder.Entity<DrugEntries>().HasKey(p => p.Id);

            builder.Entity<DrugEntries>().Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Entity<DrugEntries>().Property(p => p.Time).IsRequired();
        }
    }
}
