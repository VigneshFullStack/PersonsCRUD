using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CsvHelper;
using System.IO;
using CsvHelper.Configuration;
using OfficeOpenXml;

namespace Services
{
	public class PersonsService : IPersonsService
	{
		// private fields
		private readonly PersonsDbContext _db;
		private readonly ICountriesService _countriesService;

		// constructor
		public PersonsService(PersonsDbContext personsDbContext, ICountriesService countriesService)
		{
			_db = personsDbContext;
			_countriesService = countriesService;
		}

		public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
		{
			// check if PersonAddRequest is not null
			if (personAddRequest == null) throw new ArgumentNullException(nameof(personAddRequest));

			// model validation
			ValidationHelper.ModelValidation(personAddRequest);

			// convert PersonAddRequest into Person type
			Person person = personAddRequest.ToPerson();

			// generate PersonId
			person.PersonId = Guid.NewGuid();

			// add person object into persons list
			_db.Persons.Add(person);
			await _db.SaveChangesAsync();
			//_db.sp_InsertPerson(person);

			// convert the person object into PersonResponse type
			return person.ToPersonResponse();
		}

		public async Task<List<PersonResponse>> GetAllPersons()
		{
			// convert all persons from Person type to PersonResponse type
			// Return all PersonResponse objects.
			var persons = await _db.Persons.Include("Country").ToListAsync();
			return persons.Select(person => person.ToPersonResponse()).ToList();

			//return _db.sp_GetAllPersons().Select(person => person.ToPersonResponse()).ToList();
		}

		public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
		{
			// check if the personId is not null
			if (personId == null) return null;

			// get matching person from person list based on personId
			Person? person = await _db.Persons.Include("Country").FirstOrDefaultAsync(temp => temp.PersonId == personId);

			// check if the person object is not null
			if (person == null) return null;

			// convert matching Person object into PersonResponse type
			return person.ToPersonResponse() ?? null;
		}

		public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
		{
			List<PersonResponse> allPersons = await GetAllPersons();
			List<PersonResponse> matchingPersons = allPersons;

			// Check if "searchBy" is not null
			if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
				return matchingPersons;

			// Get matching persons from List<Person> based on given searchBy and searchString
			switch (searchBy)
			{
				case nameof(PersonResponse.PersonName):
					matchingPersons = allPersons.Where(temp =>
					(!string.IsNullOrEmpty(temp.PersonName) ?
					temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
					break;
				case nameof(PersonResponse.Email):
					matchingPersons = allPersons.Where(temp =>
					(!string.IsNullOrEmpty(temp.Email) ?
					temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
					break;
				case nameof(PersonResponse.DateOfBirth):
					matchingPersons = allPersons.Where(temp =>
					(temp.DateOfBirth != null) ?
					temp.DateOfBirth.Value.ToString("dd MMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
					break;
				case nameof(PersonResponse.Gender):
					matchingPersons = allPersons.Where(temp =>
					(!string.IsNullOrEmpty(temp.Gender) ?
					temp.Gender.StartsWith(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
					break;
				case nameof(PersonResponse.CountryId):
					matchingPersons = allPersons.Where(temp =>
					(!string.IsNullOrEmpty(temp.Country) ?
					temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
					break;
				case nameof(PersonResponse.Address):
					matchingPersons = allPersons.Where(temp =>
					(!string.IsNullOrEmpty(temp.Address) ?
					temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
					break;
				default:
					matchingPersons = allPersons;
					break;
			}

			// Return all matching PersonResponse objects
			return matchingPersons;
		}

		public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
		{
			// check if "sortBy" is not null
			if (string.IsNullOrEmpty(sortBy))
				return allPersons;

			// Get matching persons from List<Person> based on given "sortBy" and "sortOrder"
			List<PersonResponse> sortedPersons = (sortBy, sortOrder)
			switch
			{
				(nameof(PersonResponse.PersonName), SortOrderOptions.ASC) =>
				 allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.PersonName), SortOrderOptions.DESC) =>
				 allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Email), SortOrderOptions.ASC) =>
				 allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Email), SortOrderOptions.DESC) =>
				 allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) =>
				 allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

				(nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) =>
				 allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

				(nameof(PersonResponse.Age), SortOrderOptions.ASC) =>
				allPersons.OrderBy(temp => temp.Age).ToList(),

				(nameof(PersonResponse.Age), SortOrderOptions.DESC) =>
				 allPersons.OrderByDescending(temp => temp.Age).ToList(),

				(nameof(PersonResponse.Gender), SortOrderOptions.ASC) =>
				 allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Gender), SortOrderOptions.DESC) =>
				 allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Country), SortOrderOptions.ASC) =>
				allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Country), SortOrderOptions.DESC) =>
				 allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Address), SortOrderOptions.ASC) =>
				 allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Address), SortOrderOptions.DESC) =>
				 allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) =>
				 allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

				(nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) =>
				 allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

				_ => allPersons
			};

			// Returns the sorted persons list as List<PersonResponse> objects
			return sortedPersons;
		}

		public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
		{
			// Check if "personUpdateRequest" is not null
			if (personUpdateRequest == null)
				throw new ArgumentNullException(nameof(personUpdateRequest));

			// Validate all properties of "personUpdateRequest"
			ValidationHelper.ModelValidation(personUpdateRequest);

			// Get the matching "Person" object from List<Person> based on PersonId
			Person? matchingPerson = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonId == personUpdateRequest.PersonId);

			// Check if matching Person object is not null
			if (matchingPerson == null)
				throw new ArgumentException("Given personId doesn't exist");

			// Update all details from "personUpdateRequest" object to "Person" object
			matchingPerson.PersonName = personUpdateRequest.PersonName;
			matchingPerson.Email = personUpdateRequest.Email;
			matchingPerson.Gender = personUpdateRequest.Gender.ToString();
			matchingPerson.CountryId = personUpdateRequest.CountryId;
			matchingPerson.Address = personUpdateRequest.Address;
			matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
			matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
			
			await _db.SaveChangesAsync();

			// Convert the "Person" object into "PersonResponse" type
			// Return PersonResponse object with Updated details
			return matchingPerson.ToPersonResponse();
		}

		public async Task<bool> DeletePerson(Guid? personId)
		{
			// Check if "personId" is not null
			if (personId == null)
				throw new ArgumentNullException(nameof(personId));

			// Get the matching "Person" object from List<Person> based on PersonId
			Person? matchingPerson = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonId == personId);

			// Check if matching "Person" object is not null
			if (matchingPerson == null)
				return false;

			// Delete the matching "Person" object from List<Person>
			_db.Persons.Remove(await _db.Persons.FirstAsync(temp => temp.PersonId == personId));
			await _db.SaveChangesAsync();

			// Return boolean value indicating whether person object was deleted or not
			return true;
		}

		public async Task<MemoryStream> GetPersonsCSV()
		{
			MemoryStream memoryStream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(memoryStream);

			CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
			CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration);

			// Add headers
			csvWriter.WriteField(nameof(PersonResponse.PersonName));
			csvWriter.WriteField(nameof(PersonResponse.Email));
			csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
			csvWriter.WriteField(nameof(PersonResponse.Age));
			csvWriter.WriteField(nameof(PersonResponse.Gender));
			csvWriter.WriteField(nameof(PersonResponse.Country));
			csvWriter.WriteField(nameof(PersonResponse.Address));
			//csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));
			csvWriter.NextRecord();

			List<PersonResponse> persons = _db.Persons.Include("Country").Select(temp =>
			temp.ToPersonResponse()).ToList();

            foreach (PersonResponse person in persons)
            {
				csvWriter.WriteField(person.PersonName);
				csvWriter.WriteField(person.Email);
				if (person.DateOfBirth.HasValue)
					csvWriter.WriteField(person.DateOfBirth.Value.ToString("yyyy-MM-dd"));
				else
					csvWriter.WriteField("");
				csvWriter.WriteField(person.Age);
				csvWriter.WriteField(person.Gender);
				csvWriter.WriteField(person.Country);
				csvWriter.WriteField(person.Address);
				//csvWriter.WriteField(person.ReceiveNewsLetters);
				csvWriter.NextRecord();
				csvWriter.Flush();
            }

			memoryStream.Position = 0;
			return memoryStream;
		}

		public async Task<MemoryStream> GetPersonsExcel()
		{
			MemoryStream memoryStream = new MemoryStream();
			using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
			{
				ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
				worksheet.Cells["A1"].Value = "Person Name";
				worksheet.Cells["B1"].Value = "Email";
				worksheet.Cells["C1"].Value = "Date Of Birth";
				worksheet.Cells["D1"].Value = "Age";
				worksheet.Cells["E1"].Value = "Gender";
				worksheet.Cells["F1"].Value = "Country";
				worksheet.Cells["G1"].Value = "Address";
				worksheet.Cells["H1"].Value = "Receive News Letter";

				using (ExcelRange headerCells = worksheet.Cells["A1:H1"])
				{
					headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
					headerCells.Style.Font.Bold = true;
				}

				int row = 2;
				List<PersonResponse> persons = _db.Persons.Include("Country").Select(temp =>
				temp.ToPersonResponse()).ToList();

				foreach (PersonResponse person in persons)
				{
					worksheet.Cells[row, 1].Value = person.PersonName;
					worksheet.Cells[row, 2].Value = person.Email;
					if(person.DateOfBirth.HasValue)
						worksheet.Cells[row, 3].Value = person.DateOfBirth.Value.ToString("yyyy-MM-dd");
					worksheet.Cells[row, 4].Value = person.Age;
					worksheet.Cells[row, 5].Value = person.Gender;
					worksheet.Cells[row, 6].Value = person.Country;
					worksheet.Cells[row, 7].Value = person.Address;
					worksheet.Cells[row, 8].Value = person.ReceiveNewsLetters;

					row++;
				}

				worksheet.Cells[$"A1:H{row}"].AutoFitColumns();
				await excelPackage.SaveAsync();
			}

			memoryStream.Position = 0;
			return memoryStream;
		}
	}
}	
