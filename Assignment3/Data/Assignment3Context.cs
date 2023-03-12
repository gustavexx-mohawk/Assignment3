using Assignment3.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Data
{
    public class Assignment3Context : DbContext
    {
        public Assignment3Context(DbContextOptions<Assignment3Context> options)
            : base(options)
        {
        }

        private const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=A3DB;Trusted_Connection=True;MultipleActiveResultSets=true";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Patient> Patient { get; set; } = default!;

        public DbSet<Immunization> Immunization { get; set; }

        public DbSet<Organization> Organization { get; set; }

        public DbSet<Provider> Provider { get; set; }

        public DbSet<Error> Error { get; set; }
    }
}