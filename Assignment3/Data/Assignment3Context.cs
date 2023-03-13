/*
  Student names: Jongeun Kim, Gustavo Marcano Valero, Piper Sicard, and Amanda Venier.
  Student numbers: 000826393, 000812644, 000824338, 000764961
  Date: March 12, 2023

  Purpose: A DbContext that helps create the entity for the database migration.

  Statement of Authorship: We, Jongeun Kim (000826393), Gustavo Marcano Valero (000812644), Piper Sicard (000824338), and Amanda Venier (000764961) certify that this material is our original work.
                           No other person's work has been used without due acknowledgement.
*/

using Assignment3.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Data
{
    /// <summary>
    /// A DbContext that helps create the entity for the database migration.
    /// </summary>
    public class Assignment3Context : DbContext
    {
        /// <summary>
        /// A constructor for Assignment3Context that call the base constructor.
        /// </summary>
        /// <param name="options">The options to be used by Assignment3Context.</param>
        public Assignment3Context(DbContextOptions<Assignment3Context> options)
            : base(options)
        {
        }

        /// <value>
        /// The connection string for the database.
        /// </value>
        private const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=A3DB;Trusted_Connection=True;MultipleActiveResultSets=true";

        /// <summary>
        /// Configures the datavase (and other options) to be used for the current context.
        /// </summary>
        /// <param name="optionsBuilder">A builder used to modify or create the options for the current context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        /// <value>
        /// A collection of Patients in the current context.
        /// </value>
        public DbSet<Patient> Patient { get; set; } = default!;

        /// <value>
        /// A collection of Immunizations in the current context.
        /// </value>
        public DbSet<Immunization> Immunization { get; set; }

        /// <value>
        /// A collection of Organizations in the current context.
        /// </value>
        public DbSet<Organization> Organization { get; set; }

        /// <value>
        /// A collection of Providers in the current context.
        /// </value>
        public DbSet<Provider> Provider { get; set; }

        /// <value>
        /// A collection of Errors in the current context.
        /// </value>
        public DbSet<Error> Error { get; set; }
    }
}