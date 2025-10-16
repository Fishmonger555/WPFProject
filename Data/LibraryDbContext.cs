using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Library.Models;
using System.Configuration;

namespace Library.Data
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public LibraryDbContext()
        {

        }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // IMPORTANT: Replace this with your actual connection string.
                // For .NET Framework, this is typically read from App.config.
                // For this example, we'll use a placeholder and explain how to configure it.
                // If you have an App.config file, it should look something like:
                // <configuration>
                //   <connectionStrings>
                //     <add name="LibraryDatabase" connectionString="Server=(localdb)\MSSQLLocalDB;Database=LibraryDB;Trusted_Connection=True;MultipleActiveResultSets=true" providerName="System.Data.SqlClient" />
                //   </connectionStrings>
                // </configuration>

                // If you don't have App.config or want to hardcode for now (NOT recommended for production):
                // optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=LibraryDB;Trusted_Connection=True;MultipleActiveResultSets=true");

                // Best practice for .NET Framework: Read from App.config
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["LibraryDatabase"]?.ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    // Fallback if connection string is not found (or for initial setup)
                    // You MUST configure this in App.config!
                    // For example: optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=LibraryDB;Trusted_Connection=True;MultipleActiveResultSets=true");
                    throw new InvalidOperationException("Connection string 'LibraryDatabase' not found in App.config or is empty.");
                }
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Example: Fluent API configuration for Book if needed
            modelBuilder.Entity<Book>()
                .HasIndex(b => b.ISBN)
                .IsUnique();
        }
    }
}
