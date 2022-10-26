using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using ExtraFunction.Model;
using User = ExtraFunction.Model.User;

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

            optionsBuilder.UseCosmos("https://sawa-db-fabio.documents.azure.com:443/",
                            "tfGJUagGE3YBw3vCrDhreFiJn0RT0EfnS5NESBJ0ypja5MxfOgRoBFvVUiMoWgurdPzZ1kWcZ1topQrOy5Et7Q==",
                            "sawa-db-fabio");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<User>().ToContainer("Users").HasPartitionKey(c => c.Id);          
            modelBuilder.Entity<User>().ToContainer("TermsAndConditions").HasPartitionKey(c => c.Id);          
            modelBuilder.Entity<User>().ToContainer("Disclaimers").HasPartitionKey(c => c.Id);          
            modelBuilder.Entity<User>().OwnsMany(u => u.Friends);
            modelBuilder.Entity<User>().OwnsMany(u => u.Achievements);
        }
    }
}
