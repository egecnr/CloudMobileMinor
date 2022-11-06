using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using ExtraFunction.Model;
using User = ExtraFunction.Model.User;
using System;

namespace ExtraFunction.DAL
{
    public class DatabaseContext : DbContext
    {

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<TermsAndConditions> TermsAndConditions { get; set; } = null!;
        public DbSet<Disclaimers> Disclaimers { get; set; } = null!;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //secure connection string later

            optionsBuilder.UseCosmos(Environment.GetEnvironmentVariable("DBUri"),
                           Environment.GetEnvironmentVariable("DbKey"),
                           Environment.GetEnvironmentVariable("DbName"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToContainer("Users")
                .HasPartitionKey(c => c.Id);
            
            modelBuilder.Entity<TermsAndConditions>()
                .ToContainer("TermsAndConditions")
                .HasPartitionKey(c => c.Id);   
            
            modelBuilder.Entity<Disclaimers>()
                .ToContainer("Disclaimers")
                .HasPartitionKey(c => c.Id);

            modelBuilder.Entity<User>()
                .OwnsMany(u => u.Friends);

            modelBuilder.Entity<User>()
                .OwnsMany(u => u.Achievements);
        }
    }
}
