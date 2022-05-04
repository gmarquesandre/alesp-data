﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Alesp.Api.Migrations
{
    [DbContext(typeof(AlespDbContext))]
    [Migration("20220422133541_add-congressperson-email")]
    partial class addcongresspersonemail
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Alesp.Shared.CongressPerson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AlespId")
                        .HasColumnType("int");

                    b.Property<string>("AreasOfWork")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Biography")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PictureBase64")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegionDescription")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CongressPeople");
                });

            modelBuilder.Entity("Alesp.Shared.Legislature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.ToTable("Legislatures");
                });

            modelBuilder.Entity("Alesp.Shared.Presence", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CongressPersonId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("LegislativeSessionType")
                        .HasColumnType("int");

                    b.Property<int>("PresenceStatus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CongressPersonId");

                    b.HasIndex("Date", "CongressPersonId")
                        .IsUnique();

                    b.ToTable("Presences");
                });

            modelBuilder.Entity("Alesp.Shared.Provider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Identification")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.Property<int>("IdentificationType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ShareCapital")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Providers");
                });

            modelBuilder.Entity("Alesp.Shared.Spending", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CongressPersonId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProviderId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ProviderId");

                    b.HasIndex("CongressPersonId", "Date", "Type", "ProviderId")
                        .IsUnique();

                    b.ToTable("Spendings");
                });

            modelBuilder.Entity("CongressPersonLegislature", b =>
                {
                    b.Property<int>("CongressPeopleId")
                        .HasColumnType("int");

                    b.Property<int>("LegislaturesId")
                        .HasColumnType("int");

                    b.HasKey("CongressPeopleId", "LegislaturesId");

                    b.HasIndex("LegislaturesId");

                    b.ToTable("CongressPersonLegislature");
                });

            modelBuilder.Entity("Alesp.Shared.Presence", b =>
                {
                    b.HasOne("Alesp.Shared.CongressPerson", "CongressPerson")
                        .WithMany()
                        .HasForeignKey("CongressPersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CongressPerson");
                });

            modelBuilder.Entity("Alesp.Shared.Spending", b =>
                {
                    b.HasOne("Alesp.Shared.CongressPerson", "CongressPerson")
                        .WithMany()
                        .HasForeignKey("CongressPersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Alesp.Shared.Provider", "Provider")
                        .WithMany()
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CongressPerson");

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("CongressPersonLegislature", b =>
                {
                    b.HasOne("Alesp.Shared.CongressPerson", null)
                        .WithMany()
                        .HasForeignKey("CongressPeopleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Alesp.Shared.Legislature", null)
                        .WithMany()
                        .HasForeignKey("LegislaturesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
