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
using ExtraFunction.Model_;
using ShowerShow.Model;

namespace ShowerShow.DAL
{
    public class DatabaseContext : DbContext
    {

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Schedule> Schedules { get; set; } = null!;
        public DbSet<ShowerData> ShowerInstances { get; set; } = null!;
        public DbSet<TermsAndConditions> TermsAndConditions { get; set; } = null!;
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
    }
}
