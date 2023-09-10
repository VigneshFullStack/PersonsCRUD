using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReceiveNewsLetters = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryId", "CountryName" },
                values: new object[,]
                {
                    { new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), "Philippines" },
                    { new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), "India" },
                    { new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"), "USA" },
                    { new Guid("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"), "Canada" },
                    { new Guid("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5"), "Australia" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "Address", "CountryId", "DateOfBirth", "Email", "Gender", "PersonName", "ReceiveNewsLetters" },
                values: new object[,]
                {
                    { new Guid("08118bb1-d221-46c4-8e9c-996344acec13"), "707 Hickory Blvd.", new Guid("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5"), new DateTime(1993, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "sophia.moore@example.com", "Female", "Sophia Moore", true },
                    { new Guid("235c8254-0c06-4589-8d08-a11d1c546dd2"), "202 Birch Lane", new Guid("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5"), new DateTime(1975, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "william.brown@example.com", "Male", "William Brown", true },
                    { new Guid("341c4097-b6b7-405f-af0a-38a7d1f0c37b"), "1234 Elm St.", new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), new DateTime(1985, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "john.doe@example.com", "Male", "John Doe", true },
                    { new Guid("5a42e97a-6aac-487d-9222-efe17eced8a1"), "505 Fir St.", new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"), new DateTime(1978, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "angela.white@example.com", "Female", "Angela White", true },
                    { new Guid("5ab4851c-2b5a-43fa-8dc2-953a1a28a5ec"), "606 Grove Avenue", new Guid("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"), new DateTime(1999, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "james.wilson@example.com", "Male", "James Wilson", false },
                    { new Guid("7994552c-d3f5-4737-a91c-4c6579dfc178"), "5678 Oak St.", new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), new DateTime(1990, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "jane.smith@example.com", "Female", "Jane Smith", false },
                    { new Guid("ca26e084-248a-4d58-a153-4fffd7274ce6"), "101 Pine Drive", new Guid("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"), new DateTime(1988, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "emily.davis@example.com", "Female", "Emily Davis", false },
                    { new Guid("dae94dc9-b3e6-40be-b50e-6abff205370f"), "303 Cedar Place", new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), new DateTime(1992, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "jessica.taylor@example.com", "Female", "Jessica Taylor", true },
                    { new Guid("f6d75019-7492-4704-9b47-7a90e3889e68"), "404 Elmwood Drive", new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), new DateTime(1989, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "robert.lee@example.com", "Male", "Robert Lee", false },
                    { new Guid("fd5eb70c-832d-48f7-a4f8-7e8cd5b9f065"), "9 Maple Ave.", new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"), new DateTime(1995, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "michael.johnson@example.com", "Male", "Michael Johnson", true }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
