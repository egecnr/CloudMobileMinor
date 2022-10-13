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

namespace ShowerShow.DAL
{
    public class DatabaseContext : DbContext
    {

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Schedule> Schedules { get; set; } = null!;
        public DbSet<ShowerData> ShowerInstances { get; set; } = null!;
        public DbSet<Achievement> Achievements { get; set; } = null!;
        public DbSet<Preferences> Preferences { get; set; } = null!;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //secure connection string later

            optionsBuilder.UseCosmos("https://sawa-db-fabio.documents.azure.com:443/",
                            "tfGJUagGE3YBw3vCrDhreFiJn0RT0EfnS5NESBJ0ypja5MxfOgRoBFvVUiMoWgurdPzZ1kWcZ1topQrOy5Et7Q==",
                            "sawa-db-fabio");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new EnumCollectionJsonValueConverter<DayOfWeek>();
         
            modelBuilder.Entity<User>().ToContainer("Users").HasPartitionKey(c => c.Id);
            
            modelBuilder.Entity<Schedule>().ToContainer("Schedules").HasPartitionKey(c=>c.UserId);
            modelBuilder.Entity<Preferences>().ToContainer("Preferences").HasPartitionKey(c=>c.UserId);
            modelBuilder.Entity<ShowerData>().ToContainer("ShowerData").HasPartitionKey(c=>c.UserId); //This could be a date too ask Frank

            modelBuilder.Entity<User>().OwnsMany(u => u.Friends);
            modelBuilder.Entity<User>().OwnsMany(u => u.Achievements);
            modelBuilder.Entity<Schedule>().OwnsMany(s => s.Tags);
            modelBuilder
              .Entity<Schedule>()
              .Property(s => s.DaysOfWeek)
              .HasConversion(converter);
        }

        public List<Achievement> InitAchievemnets() 
        {
            List<Achievement> achievements = new List<Achievement>();

            achievements.Add(new Achievement(new Guid(), "Perfect week", "Using sawa as 7 times a week", 1, 1)); //string, string, int,int
            achievements.Add(new Achievement(new Guid(), "Great week", "Using sawa 5 times a week", 1, 1));
            achievements.Add(new Achievement(new Guid(), "Perfect month", "Using Sawa 28 times/month", 1, 1));
            achievements.Add(new Achievement(new Guid(), "Great month", "Using Sawa 20times/month", 1, 1));
            achievements.Add(new Achievement(new Guid(), "Perfect quarter", "Using sawa 84 times/quarter", 1, 1));
            achievements.Add(new Achievement(new Guid(), "Great quarter", "Using Sawa 60 times/quarter", 1, 1));
            achievements.Add(new Achievement(new Guid(), "Perfect Year", "Using Sawa 336 times/year", 1, 1));
            achievements.Add(new Achievement(new Guid(), "Great Year", "Using Sawa 240 times/year", 1, 1));
            achievements.Add(new Achievement(new Guid(), "Fresh frog", "Showering (partly) cold at least five times in a week", 1, 1));
            achievements.Add(new Achievement(new Guid(), "Early Bird", "Shower before 8:00 ", 1, 1));
            achievements.Add(new Achievement(new Guid(), "Early Bird Bonanza", "Shower before 8:00 five times a week", 1, 1));
            achievements.Add(new Achievement(new Guid(), "Night Owl", "Shower past 22:00", 1, 1));
            achievements.Add(new Achievement(new Guid(), "Night Albatros", "Shower past 22:00 five times a week", 1, 1));
            achievements.Add(new Achievement(new Guid(), "Flash Shower", "Shower in less than five minutes", 1, 1));

            return achievements;

        }
    }
}
