﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using CsvHelper;
using System.Text;

namespace PersonsCRUD.Controllers
{
	[Route("[controller]")]
	public class PersonsController : Controller
	{
		// private fields
		private readonly IPersonsService _personsService;
		private readonly ICountriesService _countriesService;

		// constructor
		public PersonsController(IPersonsService personsService, ICountriesService countriesService)
		{
			_personsService = personsService;
			_countriesService = countriesService;
		}

		[Route("[action]")]
		[Route("/")]
		public async Task<IActionResult> Index(string searchBy, string? searchString,
			string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
		{
			// Search
			ViewBag.SearchFields = new Dictionary<string, string>()
			{
				{ nameof(PersonResponse.PersonName), "Person Name" },
				{ nameof(PersonResponse.Email), "Email" },
				{ nameof(PersonResponse.DateOfBirth), "Date Of Birth" },
				{ nameof(PersonResponse.Gender), "Gender" },
				{ nameof(PersonResponse.CountryId), "Country" },
				{ nameof(PersonResponse.Address), "Address" }
			};
			List<PersonResponse> persons = await _personsService.GetFilteredPersons(searchBy, searchString);
			ViewBag.CurrentSearchBy = searchBy;
			ViewBag.CurrentSearchString = searchString;

			// Sort
			List<PersonResponse> sortedPersons = await _personsService.GetSortedPersons(persons, sortBy, sortOrder);
			ViewBag.CurrentSortBy = sortBy;
			ViewBag.CurrentSortOrder = sortOrder.ToString();

			return View(sortedPersons);
		}


		// Executes when use click on "Create Person" button (While opening the create view)
		[Route("[action]")]
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			List<CountryResponse> countries = await _countriesService.GetAllCountries();
			ViewBag.Countries = countries.Select(temp =>
			new SelectListItem()
			{
				Text = temp.CountryName,
				Value = temp.CountryId.ToString()
			});
			return View();
		}

		[Route("[action]")]
		[HttpPost]
		public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
		{
			// validation
			if (!ModelState.IsValid)
			{
				List<CountryResponse> countries = await _countriesService.GetAllCountries();
				ViewBag.Countries = countries;
				ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
				return View();
			}

			// call the service method 
			PersonResponse person_response = await _personsService.AddPerson(personAddRequest);

			// redirect to Index() action method 
			return RedirectToAction("Index", "Persons");
		}

		[Route("[action]/{personId}")]
		[HttpGet]
		public async Task<IActionResult> Edit(Guid personId)
		{
			PersonResponse? personResponse = await _personsService.GetPersonByPersonId(personId);
			if(personResponse == null)
			{
				return RedirectToAction("Index");
			}
			PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

			List<CountryResponse> countries = await _countriesService.GetAllCountries();
			ViewBag.Countries = countries.Select(temp =>
			new SelectListItem()
			{
				Text = temp.CountryName,
				Value = temp.CountryId.ToString()
			});

			return View(personUpdateRequest);
		}

		[Route("[action]/{personId}")]
		[HttpPost]
		public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
		{
			PersonResponse? personResponse = await _personsService.GetPersonByPersonId(personUpdateRequest.PersonId);
			
			if(personResponse == null)
			{
				return RedirectToAction("Index");
			}

			// validation
			if(ModelState.IsValid)
			{
				// call the service method
				PersonResponse updatedPerson = await _personsService.UpdatePerson(personUpdateRequest);
				return RedirectToAction("Index");
			} else
			{
				List<CountryResponse> countries = await _countriesService.GetAllCountries();
				ViewBag.Countries = countries;
				ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
				return View(personResponse.ToPersonUpdateRequest());
			}
		}

		[Route("[action]/{personId}")]
		[HttpGet]
		public async Task<IActionResult> Delete(Guid personId)
		{
			PersonResponse? personResponse = await _personsService.GetPersonByPersonId(personId);
			
			if(personResponse == null)
				return RedirectToAction("Index");

			return View(personResponse);
		}

		[Route("[action]/{personId}")]
		[HttpPost]
		public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateRequest)
		{
			PersonResponse? personResponse = await _personsService.GetPersonByPersonId(personUpdateRequest.PersonId);
			
			if(personResponse == null)
				return RedirectToAction("Index");

			await _personsService.DeletePerson(personResponse.PersonId);

			return RedirectToAction("Index");
		}

		[Route("PersonsPDF")]
		public async Task<ActionResult> PersonsPDF()
		{
			// Get list of persons
			List<PersonResponse> persons = await _personsService.GetAllPersons();

			// Return view as PDF
			return new ViewAsPdf("PersonsPDF", persons, ViewData)
			{
				PageMargins = new Rotativa.AspNetCore.Options.Margins()
				{
					Left = 20, Top = 20, Bottom = 20, Right = 20
				},
				PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
			};
		}

		[Route("PersonsCSV")]
		public async Task<IActionResult> PersonsCSV()
		{
			MemoryStream memoryStream = await _personsService.GetPersonsCSV();

			return File(memoryStream, "application/octet-stream", "persons.csv");
		}

		[Route("PersonsExcel")]
		public async Task<ActionResult> PersonsExcel()
		{
			MemoryStream memoryStream = await _personsService.GetPersonsExcel();

			return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
		}
	}
}
