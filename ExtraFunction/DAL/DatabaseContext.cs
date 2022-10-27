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

            optionsBuilder.UseCosmos("https://sawa-db.documents.azure.com:443/",
                        "gggcb28Z24nJAmpz4SRwQRNT9Xyd0wn1riSKAUkvVyaBf4WRALsyx4kgl6POPmi8Ka7JHZfTx06uWD3DHzoqTw==",
                        "sawa-db");
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
