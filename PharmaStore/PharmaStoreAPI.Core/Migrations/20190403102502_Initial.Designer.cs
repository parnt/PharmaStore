﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PharmaStoreAPI.Core.Models;

namespace PharmaStoreAPI.Core.Migrations
{
    [DbContext(typeof(PharmaStoreContext))]
    [Migration("20190403102502_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PharmaStoreAPI.Core.Models.Medicines.Medicine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContentQuantity")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<string>("Description");

                    b.Property<int>("MedicineTypeId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<decimal>("Price");

                    b.Property<string>("Producer")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("MedicineTypeId");

                    b.ToTable("Medicines");
                });

            modelBuilder.Entity("PharmaStoreAPI.Core.Models.Medicines.MedicineType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("MedicineTypes");
                });

            modelBuilder.Entity("PharmaStoreAPI.Core.Models.Medicines.Medicine", b =>
                {
                    b.HasOne("PharmaStoreAPI.Core.Models.Medicines.MedicineType", "MedicineType")
                        .WithMany("Medicines")
                        .HasForeignKey("MedicineTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
