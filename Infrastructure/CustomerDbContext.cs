using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Customer>()
                        .HasIndex(_ => _.Email)
                        .IsUnique();
            modelBuilder.Entity<Customer>()
                        .HasIndex(_ => new { _.FirstName, _.LastName, _.DateOfBirth })
                        .IsUnique();
        }
    }
}