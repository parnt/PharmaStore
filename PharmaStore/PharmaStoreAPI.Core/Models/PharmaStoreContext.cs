namespace PharmaStoreAPI.Core.Models
{
    using Medicines;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public partial class PharmaStoreContext : DbContext
    {
        public PharmaStoreContext()
        {
        }

        public PharmaStoreContext(DbContextOptions<PharmaStoreContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<PharmaStoreContext>();
            var connectionString = configuration.GetConnectionString("Database");

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medicine>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Medicine>()
                .Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Medicine>()
                .Property(e => e.ContentQuantity)
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder.Entity<Medicine>()
                .Property(e => e.Producer)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<MedicineType>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<MedicineType>()
                .HasMany(e => e.Medicines)
                .WithOne(e => e.MedicineType)
                .HasForeignKey(e => e.MedicineTypeId)
                .IsRequired();

            modelBuilder.Entity<MedicineType>()
                .Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<MedicineType>().HasData(
                new MedicineType {Id = 1, Name = "Liquid"},
                new MedicineType {Id = 2, Name = "Tablet"},
                new MedicineType {Id = 3, Name = "Capsules"}
            );
        }
    }
}
