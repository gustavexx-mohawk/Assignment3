using Assignment3.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Data
{
    public class Assignment3Context:DbContext
    {
        public Assignment3Context(DbContextOptions<Assignment3Context> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patient { get; set; } = default!;

        public DbSet<Immunization> Immunization { get; set; }

        public DbSet<Organization> Organization { get; set; }

        public DbSet<Provider> Provider { get; set; }
    }
}
