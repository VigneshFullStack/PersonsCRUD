﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Entities.Migrations
{
    [DbContext(typeof(PersonsDbContext))]
    [Migration("20230903161117_Alter_GetPersons_SP")]
    partial class Alter_GetPersons_SP
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Entities.Country", b =>
                {
                    b.Property<Guid>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CountryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryId");

                    b.ToTable("Countries", (string)null);

                    b.HasData(
                        new
                        {
                            CountryId = new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"),
                            CountryName = "Philippines"
                        },
                        new
                        {
                            CountryId = new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"),
                            CountryName = "India"
                        },
                        new
                        {
                            CountryId = new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"),
                            CountryName = "USA"
                        },
                        new
                        {
                            CountryId = new Guid("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"),
                            CountryName = "Canada"
                        },
                        new
                        {
                            CountryId = new Guid("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5"),
                            CountryName = "Australia"
                        });
                });

            modelBuilder.Entity("Entities.Person", b =>
                {
                    b.Property<Guid>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid?>("CountryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Gender")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PersonName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("ReceiveNewsLetters")
                        .HasColumnType("bit");

                    b.Property<string>("TIN")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PersonId");

                    b.ToTable("Persons", (string)null);

                    b.HasData(
                        new
                        {
                            PersonId = new Guid("8d9b5b66-fef5-42f1-ba16-88920787a782"),
                            Address = "1234 Elm St.",
                            CountryId = new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"),
                            DateOfBirth = new DateTime(1985, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "john.doe@example.com",
                            Gender = "Male",
                            PersonName = "John Doe",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            PersonId = new Guid("127805e8-e6fe-471c-8ecd-9009320ae6e1"),
                            Address = "5678 Oak St.",
                            CountryId = new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"),
                            DateOfBirth = new DateTime(1990, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "jane.smith@example.com",
                            Gender = "Female",
                            PersonName = "Jane Smith",
                            ReceiveNewsLetters = false
                        },
                        new
                        {
                            PersonId = new Guid("c44b8fa8-eb79-4284-8bf3-1596e6d3c666"),
                            Address = "9 Maple Ave.",
                            CountryId = new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"),
                            DateOfBirth = new DateTime(1995, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "michael.johnson@example.com",
                            Gender = "Male",
                            PersonName = "Michael Johnson",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            PersonId = new Guid("1a718211-66d9-45d8-96ce-1d14c00e1561"),
                            Address = "101 Pine Drive",
                            CountryId = new Guid("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"),
                            DateOfBirth = new DateTime(1988, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "emily.davis@example.com",
                            Gender = "Female",
                            PersonName = "Emily Davis",
                            ReceiveNewsLetters = false
                        },
                        new
                        {
                            PersonId = new Guid("dba3a9b7-286a-48d6-b45b-4bb3634ade26"),
                            Address = "202 Birch Lane",
                            CountryId = new Guid("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5"),
                            DateOfBirth = new DateTime(1975, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "william.brown@example.com",
                            Gender = "Male",
                            PersonName = "William Brown",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            PersonId = new Guid("a7607108-ec3b-4d15-aaa5-88d6bac44505"),
                            Address = "303 Cedar Place",
                            CountryId = new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"),
                            DateOfBirth = new DateTime(1992, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "jessica.taylor@example.com",
                            Gender = "Female",
                            PersonName = "Jessica Taylor",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            PersonId = new Guid("8d8849e5-958c-4038-96e5-650cb51389f9"),
                            Address = "404 Elmwood Drive",
                            CountryId = new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"),
                            DateOfBirth = new DateTime(1989, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "robert.lee@example.com",
                            Gender = "Male",
                            PersonName = "Robert Lee",
                            ReceiveNewsLetters = false
                        },
                        new
                        {
                            PersonId = new Guid("aeaf9aea-cf5a-4012-8bbc-6b320e97c171"),
                            Address = "505 Fir St.",
                            CountryId = new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"),
                            DateOfBirth = new DateTime(1978, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "angela.white@example.com",
                            Gender = "Female",
                            PersonName = "Angela White",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            PersonId = new Guid("db8c92b3-9380-4a96-a16e-c3797ce96ad9"),
                            Address = "606 Grove Avenue",
                            CountryId = new Guid("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"),
                            DateOfBirth = new DateTime(1999, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "james.wilson@example.com",
                            Gender = "Male",
                            PersonName = "James Wilson",
                            ReceiveNewsLetters = false
                        },
                        new
                        {
                            PersonId = new Guid("f8fb60b0-4404-40b2-a506-98c73176d9ac"),
                            Address = "707 Hickory Blvd.",
                            CountryId = new Guid("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5"),
                            DateOfBirth = new DateTime(1993, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "sophia.moore@example.com",
                            Gender = "Female",
                            PersonName = "Sophia Moore",
                            ReceiveNewsLetters = true
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
