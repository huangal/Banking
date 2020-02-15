using Banking.Customers.Domain;
using Microsoft.EntityFrameworkCore;

namespace Banking.Customers.Data
{
    public class CustomersContext: DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }


        public CustomersContext(DbContextOptions<CustomersContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Customer>().HasKey(p => p.Id);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Post>().ToTable("Posts");


            //modelBuilder.Entity<SamuraiBattle>().HasKey(s => new { s.SamuraiId, s.BattleId });
            //modelBuilder.Entity<Horse>().ToTable("Horses");
            //modelBuilder.Entity<SamuraiBattleStat>().HasNoKey().ToView("SamuraiBattleStats");
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{

        //    modelBuilder.Entity<User>().ToTable("Users);


        //    //base.OnModelCreating(modelBuilder);

        //    // var customers = Seed(100);

        //   // modelBuilder.Entity<Customer>()   .ToTable("Customers");
        //    modelBuilder.Entity<Customer>().HasKey(p => p.Id);
        //   // modelBuilder.Entity<Customer>().HasData(Customers);

        //}


    }
}
