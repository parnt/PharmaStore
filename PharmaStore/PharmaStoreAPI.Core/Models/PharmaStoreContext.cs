using Microsoft.EntityFrameworkCore;

namespace PharmaStoreAPI.Core.Models
{
    using Medicines;

    public class PharmaStoreContext : DbContext
    {
        public PharmaStoreContext(DbContextOptions<PharmaStoreContext> options) : base(options)
        {
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

        public virtual DbSet<Medicine> Medicines { get; set; }
        public virtual DbSet<MedicineType> MedicineTypes { get; set; }
    }
}
