using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Entities
{
	public class PersonsDbContext : DbContext
	{
		public PersonsDbContext(DbContextOptions<PersonsDbContext> options) : base(options)
		{

		}

		public DbSet<Country> Countries { get; set; }
		public DbSet<Person> Persons { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Country>().ToTable("Countries");
			modelBuilder.Entity<Person>().ToTable("Persons");

			// Seed to Countries
			modelBuilder.Entity<Country>().HasData(
			   new Country 
			   { 
				   CountryId = Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"),
				   CountryName = "Philippines" 
			   },
			   new Country 
			   { 
				   CountryId = Guid.Parse("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"),
				   CountryName = "India" 
			   },
			   new Country 
			   { 
				   CountryId = Guid.Parse("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"),
				   CountryName = "USA"
			   },
			   new Country 
			   { 
				   CountryId = Guid.Parse("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"),
				   CountryName = "Canada"
			   },
			   new Country 
			   { 
				   CountryId = Guid.Parse("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5"),
				   CountryName = "Australia"
			   });

			//string countriesJson = System.IO.File.ReadAllText("countries.json");
			//List<Country> countries = JsonSerializer.Deserialize<List<Country>>(countriesJson);

			// foreach (Country country in countries)
			// {
			// 		modelBuilder.Entity<Country>().HasData(country);
			// }

			// Seed to Persons

			modelBuilder.Entity<Person>().HasData(
			new Person
			{
				PersonId = Guid.NewGuid(),
				PersonName = "John Doe",
				Email = "john.doe@example.com",
				DateOfBirth = new DateTime(1985, 5, 20),
				Gender = "Male",
				CountryId = Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"),
				Address = "1234 Elm St.",
				ReceiveNewsLetters = true
			},
			new Person
			{
				PersonId = Guid.NewGuid(),
				PersonName = "Jane Smith",
				Email = "jane.smith@example.com",
				DateOfBirth = new DateTime(1990, 4, 15),
				Gender = "Female",
				CountryId = Guid.Parse("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"),
				Address = "5678 Oak St.",
				ReceiveNewsLetters = false
			},
			new Person
			{
				PersonId = Guid.NewGuid(),
				PersonName = "Michael Johnson",
				Email = "michael.johnson@example.com",
				DateOfBirth = new DateTime(1995, 3, 25),
				Gender = "Male",
				CountryId = Guid.Parse("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"),
				Address = "9 Maple Ave.",
				ReceiveNewsLetters = true
			},
			new Person
			{
				PersonId = Guid.NewGuid(),
				PersonName = "Emily Davis",
				Email = "emily.davis@example.com",
				DateOfBirth = new DateTime(1988, 8, 8),
				Gender = "Female",
				CountryId = Guid.Parse("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"),
				Address = "101 Pine Drive",
				ReceiveNewsLetters = false
			},
			new Person
			{
				PersonId = Guid.NewGuid(),
				PersonName = "William Brown",
				Email = "william.brown@example.com",
				DateOfBirth = new DateTime(1975, 12, 15),
				Gender = "Male",
				CountryId = Guid.Parse("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5"),
				Address = "202 Birch Lane",
				ReceiveNewsLetters = true
			},
			new Person
			{
				PersonId = Guid.NewGuid(),
				PersonName = "Jessica Taylor",
				Email = "jessica.taylor@example.com",
				DateOfBirth = new DateTime(1992, 10, 20),
				Gender = "Female",
				CountryId = Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"),
				Address = "303 Cedar Place",
				ReceiveNewsLetters = true
			},
			new Person
			{
				PersonId = Guid.NewGuid(),
				PersonName = "Robert Lee",
				Email = "robert.lee@example.com",
				DateOfBirth = new DateTime(1989, 2, 5),
				Gender = "Male",
				CountryId = Guid.Parse("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"),
				Address = "404 Elmwood Drive",
				ReceiveNewsLetters = false
			},
			new Person
			{
				PersonId = Guid.NewGuid(),
				PersonName = "Angela White",
				Email = "angela.white@example.com",
				DateOfBirth = new DateTime(1978, 6, 13),
				Gender = "Female",
				CountryId = Guid.Parse("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"),
				Address = "505 Fir St.",
				ReceiveNewsLetters = true
			},
			new Person
			{
				PersonId = Guid.NewGuid(),
				PersonName = "James Wilson",
				Email = "james.wilson@example.com",
				DateOfBirth = new DateTime(1999, 1, 10),
				Gender = "Male",
				CountryId = Guid.Parse("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"),
				Address = "606 Grove Avenue",
				ReceiveNewsLetters = false
			},
			new Person
			{
				PersonId = Guid.NewGuid(),
				PersonName = "Sophia Moore",
				Email = "sophia.moore@example.com",
				DateOfBirth = new DateTime(1993, 7, 7),
				Gender = "Female",
				CountryId = Guid.Parse("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5"),
				Address = "707 Hickory Blvd.",
				ReceiveNewsLetters = true
			});

			//string personsJson = System.IO.File.ReadAllText("persons.json");
			//List<Person> persons = JsonSerializer.Deserialize<List<Person>>(personsJson);

			// foreach (Person person in persons)
			// {
			//		modelBuilder.Entity<Person>().HasData(person);
			// }

			// Fluent API
			modelBuilder.Entity<Person>().Property(temp => temp.TIN)
				.HasColumnName("TaxIdentificationNumber")
				.HasColumnType("varchar(8)")
				.HasDefaultValue("ABC12345");

			//modelBuilder.Entity<Person>().HasIndex(temp => temp.TIN).IsUnique();

			modelBuilder.Entity<Person>().HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8");

			// table relations
			//modelBuilder.Entity<Person>(entity =>
			//{
			//	entity.HasOne<Country>(c => c.Country).WithMany(p => p.Persons)
			//	.HasForeignKey(p => p.CountryId);
			//});
		}

		public List<Person> sp_GetAllPersons()
		{
			return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
		}

		public int sp_InsertPerson(Person person)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter("@PersonId", person.PersonId),
				new SqlParameter("@PersonName", person.PersonName),
				new SqlParameter("@Email", person.Email),
				new SqlParameter("@DateOfBirth", person.DateOfBirth),
				new SqlParameter("@Gender", person.Gender),
				new SqlParameter("@CountryId", person.CountryId),
				new SqlParameter("@Address", person.Address),
				new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters)
			};
			return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @PersonId, @PersonName, @Email, @DateOfBirth, @Gender, @CountryId, @Address, @ReceiveNewsLetters", parameters);
		}
	}
}
