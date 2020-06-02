using Microsoft.EntityFrameworkCore;
using ocasAssignment.Models.Database;

namespace ocasAssignment.Data
{
    /// <summary>
    /// Application Database Context
    /// This class will setup the ocas Entities.
    /// </summary>
    public class AppDbContext : DbContext
    {

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<Event> Events { get; set; }

        public virtual DbSet<EmployeeSignUps> EmployeeSignUps { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">Application DbContext Options</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Overrides the OnModelCreating method to setup the ocas Entities.
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder object</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().HasKey(c => c.Id);
            modelBuilder.Entity<Employee>().HasIndex(c => new { c.Id, c.EmailAddress }).IsUnique();
            modelBuilder.Entity<Employee>().Property(c => c.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Employee>().Property(c => c.FirstName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Employee>().Property(c => c.LastName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Employee>().Property(c => c.EmailAddress).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Event>().HasKey(c => c.Id);
            modelBuilder.Entity<Event>().HasIndex(c => new { c.Id, c.Name }).IsUnique();
            modelBuilder.Entity<Event>().Property(c => c.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Event>().Property(c => c.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Event>().Property(c => c.StartDate).IsRequired();
            modelBuilder.Entity<Event>().Property(c => c.EndDate).IsRequired();

            modelBuilder.Entity<EmployeeSignUps>().HasKey(c => c.Id);
            modelBuilder.Entity<EmployeeSignUps>().HasIndex(c => c.Id).IsUnique();
            modelBuilder.Entity<EmployeeSignUps>().Property(c => c.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<EmployeeSignUps>().Property(c => c.Comments).HasMaxLength(500);

        }

    }
}
