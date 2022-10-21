using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using ShowerShow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedule = ShowerShow.Models.Schedule;
using User = ShowerShow.Models.User;
using DayOfWeek = ShowerShow.Models.DayOfWeek;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShowerShow.Utils;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Options;
using ShowerShow.Model;

namespace ShowerShow.DAL
{
    public class DatabaseContext : DbContext
    {

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Schedule> Schedules { get; set; } = null!;
        public DbSet<ShowerData> ShowerInstances { get; set; } = null!;
        public DbSet<Achievement> Achievements { get; set; } = null!;
        public DbSet<Preferences> Preferences { get; set; } = null!;
        public DbSet<ShowerThought> ShowerThoughts { get; set; } = null!;
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //secure connection string later

            optionsBuilder.UseCosmos("https://database-sawa.documents.azure.com:443/",
               "0iV6DDVOqBso4R7ylBYskYk7vPhYtzoQS8kg7ltSdAuTY7xpXLlHtCZAh3au9qDoEOPw4lE91jVApTkQrHLB8g==",
               "Database - SAWA");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new EnumCollectionJsonValueConverter<DayOfWeek>();
         
            modelBuilder.Entity<User>().ToContainer("Users").HasPartitionKey(c => c.Id);
            
            modelBuilder.Entity<Schedule>().ToContainer("Schedules").HasPartitionKey(c=>c.UserId);
            modelBuilder.Entity<Preferences>().ToContainer("Preferences").HasPartitionKey(c=>c.UserId);
            modelBuilder.Entity<ShowerData>().ToContainer("ShowerData").HasPartitionKey(c=>c.UserId); //This could be a date too ask Frank
            modelBuilder.Entity<ShowerThought>().ToContainer("ShowerThoughts").HasPartitionKey(c => c.UserId);
            modelBuilder.Entity<User>().OwnsMany(u => u.Friends);
            modelBuilder.Entity<User>().OwnsMany(u => u.Achievements);
            modelBuilder.Entity<Schedule>().OwnsMany(s => s.Tags);
            modelBuilder
              .Entity<Schedule>()
              .Property(s => s.DaysOfWeek)
              .HasConversion(converter);
        }
    }
}
